using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesItems : MonoBehaviour
{

    public ItemsType type;
    public int amount;
    private InventoryManager inventoryManager;
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        text = GetComponentInChildren<TMP_Text>();
        text.text = amount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inventoryManager.AddItem(type, amount);
            Destroy(gameObject);
        }
    }
}

public enum ItemsType
{
    Wood,
    Stone,
    Iron,
    Gold
}
