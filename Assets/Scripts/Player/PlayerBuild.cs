using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1) && isPlacing)
        {
            Destroy(previewObject);
            isPlacing = false;
        }
        // Update the preview object's position
        if (previewObject != null)
        UpdatePreviewPosition(previewObject);

    }

    public GameObject previewObject;
    public void SetIsPlacing(string prefab)
    {
        // Create a preview object

        Debug.Log("Prefabs location: " + "Prefabs/Turret" + prefab);
        prefabToPlace = Resources.Load<GameObject>("Prefabs/Turret/" + prefab);
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
        GameObject preview = Instantiate(prefabToPlace);
        previewObject = preview;
        //disable Turnet script
        previewObject.GetComponent<Turret>().enabled = false;
        previewObject.GetComponent<BoxCollider2D>().isTrigger = true;
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

        GameObject rangeIndicator = prefab.transform.Find("Range").gameObject;
        rangeIndicator.transform.localScale = new Vector3(0, 0, 1);
        rangeIndicator.transform.localScale = new Vector3(prefab.GetComponent<Turret>().data.range * 2.5f, prefab.GetComponent<Turret>().data.range * 2.5f, 1);

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
        turret.transform.GetComponent<Turret>().enabled = true;
    }

    bool CheckPosition(Vector3 position)
    {
        List<InventoryItem> itemsNeeded = previewObject.GetComponent<Turret>().itemNeeded;
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
        
        // Check if the position is valid
        return true;
    }
}