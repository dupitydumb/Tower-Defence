using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesItem : MonoBehaviour
{
    public ItemsType type;
    private GameObject player;

    private InventoryManager inventoryManager;
    public float cutTime = 2.0f;
    private float cutTimer = 0.0f;

    private Slider progressBar;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        player = GameObject.FindWithTag("Player");
        progressBar = gameObject.GetComponentInChildren<Slider>();
        progressBar.maxValue = cutTime;
        progressBar.value = cutTimer;
    }

    // Update is called once per frame
    void Update()
    {
        CutTree();
    }


    void CutTree()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 1f)
        {
            progressBar.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.R))
            {
                //Debug.Log("Cutting tree");
                cutTimer += Time.deltaTime;
                progressBar.value = cutTimer;
                if (cutTimer >= cutTime)
                {
                    switch (type)
                    {
                        case ItemsType.Wood:
                            SpawnWood();
                            break;
                        case ItemsType.Stone:
                            AddStone();
                            break;
                        default:
                            break;
                    }
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            progressBar.gameObject.SetActive(false);
            cutTimer = 0.0f; // Reset the timer if the player moves away
            progressBar.value = cutTimer; // Reset the progress bar
        }

    }

    void SpawnWood()
    {
        GameObject wood = Instantiate(Resources.Load("Prefabs/Items/Wood"), transform.position, Quaternion.identity) as GameObject;
        wood.GetComponent<ResourcesItems>().amount = Random.Range(10, 25);
    }

    void AddStone()
    {
        inventoryManager.AddItem(type, Random.Range(4, 10));   
    }
}
