using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data.Common;
using UnityEngine.Tilemaps;
using UnityEditor;

public class PlayerBuild : MonoBehaviour
{
    public AudioClip buildSound;
    private AudioSource audioSource;
    public GameObject prefabToPlace; // The prefab to place
    public Grid grid; // Reference to the grid
    public Transform player; // Reference to the player's transform
    public float distanceInFront = 1.0f; // Distance in front of the player to place the prefab
    private InventoryManager inventoryManager; // Reference to the inventory manager
    public bool isPlacing;
    public HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();
    public Dictionary<Vector3Int, GameObject> wallObjects = new Dictionary<Vector3Int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        wallPositions = new HashSet<Vector3Int>();
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        tilemap = GameObject.FindObjectOfType<Tilemap>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState == GameState.Night)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1) && isPlacing)
        {
            Destroy(previewObject);
            isPlacing = false;
        }
        // Update the preview object's position
        if (previewObject != null)
        UpdatePreviewPosition(previewObject);
    }

    private BuildType currentBuildType;
    public GameObject previewObject;
    public void SetIsPlacing(ItemsData items)
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
        isPlacing = true;
        prefabToPlace = items.itemPrefab;
        previewObject = Instantiate(items.itemPrefab);
        previewObject.transform.SetParent(transform);
        previewObject.GetComponent<BoxCollider2D>().isTrigger = true;
        previewObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        
    }

    void UpdatePreviewPosition(GameObject prefab)
    {
        // Calculate the position in mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grid.WorldToCell(mousePosition);
        Vector3 position = grid.GetCellCenterWorld(cellPosition);
        position.z = 0;
        // Set the preview object's position
        prefab.transform.position = position;

        

        if (CheckPosition(position))
        {
            previewObject.GetComponent<SpriteRenderer>().color = new Color(1, 181, 5, 0.5f);
            if (Input.GetMouseButtonDown(0))
            {
                PlacePrefab();
            }
        }
        else
        {
            previewObject.GetComponent<SpriteRenderer>().color = new Color(181, 0, 5, 0.5f);
        }
    }

    void PlacePrefab()
    {
        audioSource.PlayOneShot(buildSound);
        // Instantiate the prefab at the preview object's position
        inventoryManager.RemoveItem(prefabToPlace.GetComponent<Wall>().itemData, 1);
        GameObject building = Instantiate(prefabToPlace, previewObject.transform.position, Quaternion.identity);
        wallObjects.Add(grid.WorldToCell(previewObject.transform.position), building);
        building.GetComponent<Wall>().Init();
        wallPositions.Add(grid.WorldToCell(previewObject.transform.position));
        building.transform.GetComponent<BoxCollider2D>().enabled = true;
        building.GetComponent<Wall>().gridPos = grid.WorldToCell(previewObject.transform.position);
        building.GetComponent<Wall>().SetWallVariants(grid.WorldToCell(previewObject.transform.position));
        building.GetComponent<Wall>().UpdateNeighborWalls();
        Debug.LogWarning(wallPositions.Count);
        Debug.Log("Placed building at " + grid.WorldToCell(previewObject.transform.position));
        // Deduct the items needed from the inventory
    }

    private Tilemap tilemap;
    bool CheckPosition(Vector3 position)
    {   
        // check if player has the item
        if (inventoryManager.GetAmount(prefabToPlace.GetComponent<Wall>().itemData) <= 0)
        {
            Destroy(previewObject);
            isPlacing = false;
            return false;
        }
        
        if (previewObject.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Obstacle")))
        {
            return false;
        }
        // if collied with any other box collider
        if (previewObject.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Wall")))
        {
            return false;
        }

        
        // Check if the position is valid
        return true;
    }
}

[CustomEditor(typeof(PlayerBuild))]
public class PlayerBuildEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerBuild playerBuild = (PlayerBuild)target;

        EditorGUILayout.LabelField("Wall Positions", EditorStyles.boldLabel);

        if (playerBuild.wallPositions != null)
        {
            foreach (Vector3Int position in playerBuild.wallPositions)
            {
                EditorGUILayout.LabelField(position.ToString());
            }
        }
        else
        {
            EditorGUILayout.LabelField("No wall positions");
        }

        //Addd button to refresh wall positions
        if (GUILayout.Button("Refresh Wall Positions"))
        {
            
        }
    }
}