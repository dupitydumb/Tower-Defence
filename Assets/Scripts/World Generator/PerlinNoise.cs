using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoise : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float scale = 1.0f;
    public GameObject[] treePrefab;
    public GameObject[] stone;
    public float treeThreshold = 0.5f;
    public float stoneThreshold = 0.4f;

    private Tilemap tilemap;
    private Grid grid;
    public GameObject perlinTilePrefab;
    private GameObject enviromentParent;

    private Dictionary<Vector3Int, GameObject> perlinTiles = new Dictionary<Vector3Int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //Get component type of Grid
        grid = GameObject.FindObjectOfType<Grid>();
        tilemap = GameObject.FindObjectOfType<Tilemap>();
        enviromentParent = new GameObject("Enviroment");
        GenerateNoise();
    }


    void GenerateNoise()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float xCoord = (float)x / gridWidth * scale;
                float yCoord = (float)y / gridHeight * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                // x, y coordinates of the grid
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                // Instantiate the perlinTilePrefab
                GameObject perlinTile = Instantiate(perlinTilePrefab, grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                perlinTile.transform.parent = transform;
                // Set the color of the perlinTile
                perlinTile.GetComponent<SpriteRenderer>().color = new Color(sample, sample, sample);
                // Check if the sample is greater than the treeThreshold
                if (sample > treeThreshold)
                {
                    //Check tilemapRenderer, is there any tilemap
                    if (!tilemap.HasTile(cellPosition) && !perlinTiles.ContainsKey(cellPosition))
                    {
                        GameObject trees = Instantiate(treePrefab[Random.Range(0, treePrefab.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
                        perlinTiles.Add(cellPosition, perlinTile);
                        trees.transform.parent = enviromentParent.transform;
                    }
                    
                }
                // Check if the sample is greater than the stoneThreshold
                if (sample > stoneThreshold)
                {
                    //Check tilemapRenderer, is there any tilemap
                    if (!tilemap.HasTile(cellPosition) && !perlinTiles.ContainsKey(cellPosition))
                    {
                        GameObject stones = Instantiate(stone[Random.Range(0, stone.Length)], grid.GetCellCenterWorld(cellPosition), Quaternion.identity) as GameObject;
                        perlinTiles.Add(cellPosition, perlinTile);
                        stones.transform.parent = enviromentParent.transform;
                    }
                }
            }
        }
        this.gameObject.SetActive(false);
    }
}