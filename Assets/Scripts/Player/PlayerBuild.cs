using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data.Common;
using UnityEngine.Tilemaps;

public class PlayerBuild : MonoBehaviour
{
    public GameObject prefabToPlace; // The prefab to place
    public Grid grid; // Reference to the grid
    public Transform player; // Reference to the player's transform
    public float distanceInFront = 1.0f; // Distance in front of the player to place the prefab
    private InventoryManager inventoryManager; // Reference to the inventory manager
    public bool isPlacing;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        tilemap = GameObject.FindObjectOfType<Tilemap>();
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
    public void SetIsPlacing(string prefab, BuildType buildType)
    {
        if (GameManager.instance.gameState == GameState.Night)
        {
            return;
        }
        // Create a preview object
        Debug.Log("Prefabs location: " + "Prefabs/Turret" + prefab);
        switch (buildType)
        {
            case BuildType.Turret:
                prefabToPlace = Resources.Load<GameObject>("Prefabs/Turret/" + prefab);
                currentBuildType = BuildType.Turret;
                break;
            case BuildType.Building:
                prefabToPlace = Resources.Load<GameObject>("Prefabs/Building/" + prefab);
                currentBuildType = BuildType.Building;
                break;
            default:
                break;
        }

        if (previewObject != null)
        {
            Destroy(previewObject);
        }
        GameObject preview = Instantiate(prefabToPlace);
        previewObject = preview;
        //disable Turnet script
        if (buildType == BuildType.Turret)
        {
            previewObject.GetComponent<Turret>().isEnable = false;
            
        }
        //disable BuildingMining script
        if (buildType == BuildType.Building)
        {
            previewObject.GetComponent<BuildingMining>().isEnable = false;
        }
        previewObject.GetComponent<BoxCollider2D>().isTrigger = true;
        previewObject.transform.Find("Canvas").gameObject.SetActive(true);
        isPlacing = true;
    }

    void UpdatePreviewPosition(GameObject prefab)
    {
        // Calculate the position in front of the player
        Vector3 positionInFront = player.position + player.forward * distanceInFront;
        // Snap the position to the grid
        Vector3Int cellPosition = grid.WorldToCell(positionInFront);
        Vector3 snappedPosition = grid.GetCellCenterWorld(cellPosition);
        // Update the preview object's position
        prefab.transform.position = snappedPosition;

        if (currentBuildType == BuildType.Turret)
        {
            GameObject rangeIndicator = prefab.transform.Find("Range").gameObject;
            rangeIndicator.transform.localScale = new Vector3(0, 0, 1);
            rangeIndicator.transform.localScale = new Vector3(prefab.GetComponent<Turret>().data.range[0] * 2.5f, prefab.GetComponent<Turret>().data.range[0] * 2.5f, 1);
        }

        GameObject validSprite = prefab.transform.Find("Valid").gameObject;
        validSprite.SetActive(true);
        //Check if the position is valid & items are enough
        if (CheckPosition(snappedPosition))
        {
            
            validSprite.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 0.3f);
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlacePrefab();
                isPlacing = false;
                Destroy(previewObject);
            }
        }
        else
        {
            validSprite.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.3f);
        }
    }

    void PlacePrefab()
    {
        // Instantiate the prefab at the preview object's position
        GameObject turret = Instantiate(prefabToPlace, previewObject.transform.position, Quaternion.identity);
        turret.transform.GetComponent<BoxCollider2D>().enabled = true;
        if (currentBuildType == BuildType.Turret)
        {
            turret.transform.GetComponent<Turret>().isEnable = true;
            foreach (var item in turret.transform.GetComponent<Turret>().itemNeeded)
            {
                inventoryManager.RemoveItem(item.type, item.amount);
            }
        }
        if (currentBuildType == BuildType.Building)
        {
            turret.transform.GetComponent<BuildingMining>().isEnable = true;
            foreach (var item in turret.transform.GetComponent<BuildingMining>().itemNeeded)
            {
                inventoryManager.RemoveItem(item.type, item.amount);
            }
        }
        // Deduct the items needed from the inventory
    }

    private Tilemap tilemap;
    bool CheckPosition(Vector3 position)
    {   
        //Check what layer the preview object is on
        

        List<InventoryItem> itemsNeeded = new List<InventoryItem>();
        if (currentBuildType == BuildType.Turret)
        {
            itemsNeeded = previewObject.GetComponent<Turret>().itemNeeded;
        }
        else
        {
            itemsNeeded = previewObject.GetComponent<BuildingMining>().itemNeeded;
        }
        if (previewObject.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Obstacle")))
        {
            return false;
        }
        foreach (InventoryItem item in itemsNeeded)
        {
            InventoryItem inventoryItem = inventoryManager.items.Find(i => i.type == item.type);
            if (inventoryItem == null || inventoryItem.amount < item.amount)
            {
                return false;
            }
        }
        if (currentBuildType == BuildType.Building)
        {
            BoxCollider2D collider = previewObject.GetComponent<BoxCollider2D>();
            switch (previewObject.GetComponent<BuildingMining>().type)
            {
                case ItemsType.Iron:
                    // If collider is ontop of gold collider
                    if (!collider.IsTouchingLayers(LayerMask.GetMask("Iron")))
                    {
                        return false;
                    }
                    break;
                case ItemsType.Gold:
                    if (!collider.IsTouchingLayers(LayerMask.GetMask("Gold")))
                    {
                        return false;
                    }
                    break;
                    
            }
        }

        
        // Check if the position is valid
        return true;
    }
}