using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoise : MonoBehaviour
{
    public int chunkSize = 10;
    public float scale = 1.0f;
    public GameObject[] treePrefab;
    public GameObject[] stonePrefab;
    public GameObject[] grassPrefab;
    public float treeThreshold = 0.5f;
    public float stoneThreshold = 0.4f;
    public float grassThreshold = 0.7f;
    public GameObject perlinTilePrefab;
    public Tilemap tilemap;
    public Grid grid;
    public Transform player;
    public float generationDistance = 20.0f;

    private Transform environmentParent;
    private Transform objectPoolParent;
    private HashSet<Vector2Int> generatedChunks = new HashSet<Vector2Int>();
    private List<GameObject> chunks = new List<GameObject>();

    private ChunkManager chunkManager;
    List<GameObject> perlinTilesPool = new List<GameObject>();
    List<GameObject> treesPool = new List<GameObject>();
    List<GameObject> stonesPool = new List<GameObject>();
    List<GameObject> grassPool = new List<GameObject>();


    void Start()
    {
        grid = GameObject.FindObjectOfType<Grid>();
        environmentParent = new GameObject("Environment").transform;
        objectPoolParent = new GameObject("UnusedPerlinTile").transform;
        player = GameObject.FindWithTag("Player").transform;
        chunkManager = GetComponent<ChunkManager>();
        Vector2Int pos = new Vector2Int(Mathf.FloorToInt(player.position.x / chunkSize), Mathf.FloorToInt(player.position.y / chunkSize));
        StartCoroutine(GeneratorEnviromentPool(100, treePrefab[UnityEngine.Random.Range(0, treePrefab.Length)], treesPool));
        StartCoroutine(GeneratorEnviromentPool(100, stonePrefab[UnityEngine.Random.Range(0, stonePrefab.Length)], stonesPool));
        StartCoroutine(GeneratorEnviromentPool(500, grassPrefab[UnityEngine.Random.Range(0, grassPrefab.Length)], grassPool));
    }


    bool isEnvironmentPoolEmpty()
    {
        return perlinTilesPool.Count < 100 || treesPool.Count < 100 || stonesPool.Count < 100 || grassPool.Count < 100;
    }
    void Update()
    {
        if (perlinTilesPool.Count < 100)
        {
            StartCoroutine(GeneratorEnviromentPool(20, perlinTilePrefab, perlinTilesPool));
        }
        if (treesPool.Count < 100)
        {
            StartCoroutine(GeneratorEnviromentPool(20, treePrefab[UnityEngine.Random.Range(0, treePrefab.Length)], treesPool));
        }
        if (stonesPool.Count < 100)
        {
            StartCoroutine(GeneratorEnviromentPool(20, stonePrefab[UnityEngine.Random.Range(0, stonePrefab.Length)], stonesPool));
        }
        if (grassPool.Count < 100)
        {
            StartCoroutine(GeneratorEnviromentPool(20, grassPrefab[UnityEngine.Random.Range(0, grassPrefab.Length)], grassPool));
        }

        if (isEnvironmentPoolEmpty())
        {
            return;
        }

        GenerateChunksNearPlayer();
    }

    void GenerateChunksNearPlayer()
    {
        // Check if chunk already exists
        foreach (GameObject chunk in chunks)
        {
            if (chunk.GetComponent<Chunk>().position == new Vector2Int(Mathf.FloorToInt(player.position.x / chunkSize), Mathf.FloorToInt(player.position.y / chunkSize)))
            {
                chunk.SetActive(true);
                return;
            }
        }

        Vector3 playerPosition = player.position;
        Vector2Int playerChunk = new Vector2Int(Mathf.FloorToInt(playerPosition.x / chunkSize), Mathf.FloorToInt(playerPosition.y / chunkSize));

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int chunkPosition = new Vector2Int(playerChunk.x + x, playerChunk.y + y);
                if (!generatedChunks.Contains(chunkPosition))
                {
                    float distanceToPlayer = Vector2.Distance(playerPosition, chunkPosition * chunkSize);
                    if (distanceToPlayer <= generationDistance)
                    {
                        transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(GenerateChunk(chunkPosition, chunkSize));
                        generatedChunks.Add(chunkPosition);
                    }
                }
            }
        }
    }

    IEnumerator GeneratorEnviromentPool(int amount, GameObject prefab, List<GameObject> pool)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            obj.transform.parent = environmentParent;
            pool.Add(obj);
            yield return null;
        }
    }
    IEnumerator GenerateChunk(Vector2Int chunkPosition, int chunkSize)
    {
        GameObject chunk = new GameObject("Chunk " + chunkPosition);
        chunk.transform.parent = transform;
        chunk.transform.position = new Vector3(chunkPosition.x * chunkSize, chunkPosition.y * chunkSize, 0);
        chunk.AddComponent<Chunk>();
        chunk.GetComponent<Chunk>().position = chunkPosition;
        chunk.GetComponent<Chunk>().enableDistance = generationDistance + 10;
        chunks.Add(chunk);
        //Load environment background from resources
        Instantiate(Resources.Load<GameObject>("Prefabs/Enviroment/Desert"), chunk.transform.position, Quaternion.identity, chunk.transform);
        int batchSize = 100;
        int count = 0;
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                // Calculate unique Perlin noise coordinates for each position
                float xCoord = ((chunkPosition.x * chunkSize + x) / (float)chunkSize * scale);
                float yCoord = ((chunkPosition.y * chunkSize + y) / (float)chunkSize * scale);
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Vector3Int cellPosition = new Vector3Int(chunkPosition.x * chunkSize + x, chunkPosition.y * chunkSize + y, 0);
                if (sample > treeThreshold)
                {
                    // 70% chance to spawn a tree
                    if (Random.value < 0.7f)
                    {
                        if (treesPool.Count > 0)
                        {
                            GameObject trees = treesPool[Random.Range(0, treesPool.Count)];
                            trees.transform.position = grid.GetCellCenterWorld(cellPosition);
                            trees.transform.parent = chunk.transform;
                            trees.SetActive(true);
                            treesPool.Remove(trees);
                        }
                        else
                        {
                            GameObject grass = Instantiate(grassPrefab[Random.Range(0, grassPrefab.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                            grass.transform.position = grid.GetCellCenterWorld(cellPosition);
                            grass.transform.parent = chunk.transform;
                            
                        }
                        
                    }
                    else
                    {
                        if (stonesPool.Count > 0)
                        {
                            GameObject stone = stonesPool[Random.Range(0, stonesPool.Count)];
                            stone.transform.position = grid.GetCellCenterWorld(cellPosition);
                            stone.transform.parent = chunk.transform;
                            stone.SetActive(true);
                            stonesPool.Remove(stone);
                        }
                        else
                        {
                            GameObject grass = Instantiate(grassPrefab[Random.Range(0, grassPrefab.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                            grass.transform.position = grid.GetCellCenterWorld(cellPosition);
                            grass.transform.parent = chunk.transform;
                            grass.SetActive(true);
                        }
                    }
                }
                else if (sample > stoneThreshold)
                {
                    // 70% chance to spawn a stone
                    if (Random.value < 0.7f)
                    {
                        if (stonesPool.Count > 0)
                        {
                            GameObject stone = stonesPool[Random.Range(0, stonesPool.Count)];
                            stone.transform.position = grid.GetCellCenterWorld(cellPosition);
                            stone.transform.parent = chunk.transform;
                            stone.SetActive(true);
                            stonesPool.Remove(stone);
                        }
                        else
                        {
                            GameObject grass = Instantiate(grassPrefab[Random.Range(0, grassPrefab.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                            grass.transform.position = grid.GetCellCenterWorld(cellPosition);
                            grass.transform.parent = chunk.transform;
                            grass.SetActive(true);
                        }
                    }
                    else
                    {
                        if (treesPool.Count > 0)
                        {
                            GameObject trees = treesPool[Random.Range(0, treesPool.Count)];
                            trees.transform.position = grid.GetCellCenterWorld(cellPosition);
                            trees.transform.parent = chunk.transform;
                            trees.SetActive(true);
                            treesPool.Remove(trees);
                        }
                        else
                        {
                            GameObject grass = Instantiate(grassPrefab[Random.Range(0, grassPrefab.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                            grass.transform.position = grid.GetCellCenterWorld(cellPosition);
                            grass.transform.parent = chunk.transform;
                            grass.SetActive(true);
                        }
                    }
                }
                else if (sample > grassThreshold)
                {
                    //70% chance to spawn grass
                    if (Random.value < 0.5f)
                    {
                        if (grassPool.Count > 0)
                        {
                            GameObject grass = grassPool[Random.Range(0, grassPool.Count)];
                            grass.transform.position = grid.GetCellCenterWorld(cellPosition);
                            grass.transform.parent = chunk.transform;
                            grass.SetActive(true);
                            grassPool.Remove(grass);
                        }
                        else
                        {
                            GameObject grass = Instantiate(grassPrefab[Random.Range(0, grassPrefab.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                            grass.transform.position = grid.GetCellCenterWorld(cellPosition);
                            grass.transform.parent = chunk.transform;
                            grass.SetActive(true);
                        }
                    }
                    else if (Random.value < 0.7f)
                    {
                        if (stonesPool.Count > 0)
                        {
                            GameObject stone = stonesPool[Random.Range(0, stonesPool.Count)];
                            stone.transform.position = grid.GetCellCenterWorld(cellPosition);
                            stone.transform.parent = chunk.transform;
                            stone.SetActive(true);
                            stonesPool.Remove(stone);
                        }
                        else
                        {
                            GameObject grass = Instantiate(grassPrefab[Random.Range(0, grassPrefab.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                            grass.transform.position = grid.GetCellCenterWorld(cellPosition);
                            grass.transform.parent = chunk.transform;
                            grass.SetActive(true);
                        }
                    }
                    else if (Random.value < 0.8f)
                    {
                        if (treesPool.Count > 0)
                        {
                            GameObject trees = treesPool[Random.Range(0, treesPool.Count)];
                            trees.transform.position = grid.GetCellCenterWorld(cellPosition);
                            trees.transform.parent = chunk.transform;
                            trees.SetActive(true);
                            treesPool.Remove(trees);
                        }
                        else
                        {
                            GameObject trees = Instantiate(treePrefab[Random.Range(0, treePrefab.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                            trees.transform.position = grid.GetCellCenterWorld(cellPosition);
                            trees.transform.parent = chunk.transform;
                            trees.SetActive(true);
            
                        }
                    }
                }
                count++;
                if (count >= batchSize)
                {
                    count = 0;
                    yield return null; // Yield execution to spread the workload across frames
                }
                
            }
        }
        chunkManager.AddChunk(chunk.GetComponent<Chunk>());
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ReturnPerlinTileToPool(GameObject perlinTile)
    {
        transform.parent = gameObject.transform.Find("UnusedPerlinTile");
        perlinTile.SetActive(false);
        perlinTilesPool.Add(perlinTile);
    }
}