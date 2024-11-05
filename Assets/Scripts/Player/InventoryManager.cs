using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public List<InventoryItem> items = new List<InventoryItem>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(ItemsType type, int amount)
    {
        InventoryItem item = items.Find(i => i.type == type);
        if (item != null)
        {
            item.amount += amount;
        }
        else
        {
            InventoryItem newItem = new InventoryItem();
            newItem.type = type;
            newItem.amount = amount;
            items.Add(newItem);
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public Sprite icon;
    public ItemsType type;
    public int amount;
}
