using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    public TerrainData[] terrainDatas;
    private Grid grid;
    public int width = 100;
    public int height = 100;
    public Tilemap[] terrainTilemap;

    [Header("Generation Settings Perlin Noise")]
    public float scale = 20f;
    public string seed;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateTerrain(Vector2Int chunkPosition, int chunkSize)
    {
        StartCoroutine(GenerateNoiseMap(chunkPosition, chunkSize));
    }
    //Generate noise map
    public IEnumerator GenerateNoiseMap(Vector2Int chunkPosition, int chunkSize)
    {
        Debug.Log("Generating terrain at: " + chunkPosition);
        float[,] noiseMap = new float[width, height];

        System.Random prng = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = ((chunkPosition.x * chunkSize + x) / (float)chunkSize * scale);
                float yCoord = ((chunkPosition.y * chunkSize + y) / (float)chunkSize * scale);
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                
                Vector3Int cellPosition = new Vector3Int(chunkPosition.x * chunkSize + x, chunkPosition.y * chunkSize + y, 0);

                foreach (TerrainData terrainData in terrainDatas)
                {
                    foreach (Tilemap tilemap in terrainTilemap)
                    {
                        if (sample >= terrainData.minThreshold && sample <= terrainData.maxThreshold && tilemap.name == terrainData.layerName)
                        {
                            tilemap.SetTile(cellPosition, terrainData.tile);
                        }
                    }
                }

            }
        }


        yield return null;
    }
}
