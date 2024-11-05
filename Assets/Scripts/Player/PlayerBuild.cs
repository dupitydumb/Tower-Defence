using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    public GameObject prefabToPlace; // The prefab to place
    public Grid grid; // Reference to the grid
    public Transform player; // Reference to the player's transform
    public float distanceInFront = 1.0f; // Distance in front of the player to place the prefab

    public bool isPlacing;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the preview object
    }

    // Update is called once per frame
    void Update()
    {


        // Update the preview object's position
        if (previewObject != null)
        UpdatePreviewPosition(previewObject);
        // Check if the player presses the key to place the prefab
        if (Input.GetKeyDown(KeyCode.E)) // Change the key as needed
        {
            PlacePrefab();
        }

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
        previewObject.GetComponent<BoxCollider2D>().enabled = false;
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
    }

    void PlacePrefab()
    {
        // Instantiate the prefab at the preview object's position
        GameObject turret = Instantiate(prefabToPlace, previewObject.transform.position, Quaternion.identity);
        turret.transform.GetComponent<BoxCollider2D>().enabled = true;
        turret.transform.GetComponent<Turret>().enabled = true;
    }
}