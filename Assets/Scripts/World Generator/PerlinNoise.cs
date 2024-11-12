using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PerlinNoise : MonoBehaviour
{
    public int chunkSize = 10;
    public float scale = 1.0f;
    public GameObject perlinTilePrefab;
    public Tilemap tilemap;
    public Grid grid;
    public Transform player;
    public float generationDistance = 20.0f;

    public ObjectData[] objectData;
    public TerrainData[] terrainData;
    private Transform environmentParent;
    private Transform objectPoolParent;
    private HashSet<Vector2Int> generatedChunks = new HashSet<Vector2Int>();
    private HashSet<Vector3Int> generatedObjects = new HashSet<Vector3Int>();
    private List<GameObject> chunks = new List<GameObject>();
    private ChunkManager chunkManager;


    void Start()
    {
        Application.targetFrameRate = 60;
        grid = GameObject.FindObjectOfType<Grid>();
        environmentParent = new GameObject("Environment").transform;
        objectPoolParent = new GameObject("objectPoolParent").transform;
        player = GameObject.FindWithTag("Player").transform;
        chunkManager = GetComponent<ChunkManager>();
        Vector2Int pos = new Vector2Int(Mathf.FloorToInt(player.position.x / chunkSize), Mathf.FloorToInt(player.position.y / chunkSize));
    }


    bool isGeneratinPool = false;
    void Update()
    {
        if (!isGeneratinPool)
        {
            GenerateChunksNearPlayer();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (ObjectData objData in objectData)
            {
                GameObject obj = objData.pool.Find(o => !o.activeSelf);
                if (obj == null)
                {
                    StartCoroutine(GenerateObjectPool(objData));
                    Debug.Log(objData.pool.Count);
                }
            }
        }
    }

    void GenerateChunksNearPlayer()
    {

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

    IEnumerator GenerateObjectPool(ObjectData objData)
    {
        isGeneratinPool = true;
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(objData.prefabVariant[Random.Range(0, objData.prefabVariant.Length)], Vector3.zero, Quaternion.identity, objectPoolParent);
            obj.SetActive(false);
            objData.pool.Add(obj);
            yield return null;
        }
        isGeneratinPool = false;
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
        
        //Set batch size depending on current frame rate
        int batchSize = Application.targetFrameRate > 0 ? chunkSize * chunkSize / Application.targetFrameRate : chunkSize * chunkSize;
        int count = 0;

        Dictionary<Vector3Int, List<Vector3Int>> terrainGroups = new Dictionary<Vector3Int, List<Vector3Int>>();

        List<GameObject> objectsInChunk = new List<GameObject>();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                // Calculate unique Perlin noise coordinates for each position
                float xCoord = ((chunkPosition.x * chunkSize + x) / (float)chunkSize * scale);
                float yCoord = ((chunkPosition.y * chunkSize + y) / (float)chunkSize * scale);
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Vector3Int cellPosition = new Vector3Int(chunkPosition.x * chunkSize + x, chunkPosition.y * chunkSize + y, 0);

                if (terrainData != null)
                {
                    foreach (TerrainData terrain in terrainData)
                    {
                        if (sample > terrain.threshold)
                        {
                            if (!terrainGroups.ContainsKey(cellPosition))
                            {
                                terrainGroups.Add(cellPosition, new List<Vector3Int>());
                            }
                            terrainGroups[cellPosition].Add(cellPosition);
                        }
                    }
                }
                

                foreach (ObjectData objData in objectData)
                {
                    if (sample > objData.threshold && Random.Range(0.0f, 1.0f) < objData.percentaseToSpawn)
                    {
                        GameObject obj = objData.pool.Find(o => !o.activeSelf);
                        Debug.LogWarning(obj);
                        if (obj == null)
                        {
                            Debug.Log("Creating new object");
                            obj = Instantiate(objData.prefabVariant[Random.Range(0, objData.prefabVariant.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity, chunk.transform);
                            obj.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                            objData.pool.Add(obj);
                        }
                        else
                        {
                            Debug.Log("Reusing object");
                            obj.transform.position = grid.GetCellCenterWorld(cellPosition);
                            obj.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                            obj.transform.SetParent(chunk.transform);
                            obj.SetActive(true);
                        }
                        objectsInChunk.Add(obj);
                    }
                    
                    generatedObjects.Add(cellPosition);
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

}

[System.Serializable]
public class TerrainData
{
    public string name;
    public float threshold;
    public GameObject prefab;

    public List<GameObject> pool;
    public TerrainData(string name, float threshold, GameObject prefab, List<GameObject> pool)
    {
        this.name = name;
        this.threshold = threshold;
        this.prefab = prefab;
        this.pool = pool;
        prefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Enviroment/Terrain/" + name);
    }
}
[System.Serializable]
public class ObjectData
{
    public List<GameObject> pool;
    public float threshold;

    public GameObject[] prefabVariant;
    public GameObject prefab;
    public float percentaseToSpawn;
    public float offset;
    public ObjectData(string name, float threshold, GameObject prefab, List<GameObject> pool, float percentaseToSpawn, float offset)
    {
        this.threshold = threshold;
        this.prefab = prefabVariant[Random.Range(0, prefabVariant.Length)];
        this.pool = pool;
        this.percentaseToSpawn = percentaseToSpawn;
        this.offset = offset;
    }
}