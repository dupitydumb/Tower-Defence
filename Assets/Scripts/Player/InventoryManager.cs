using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{

    public List<InventoryItem> items = new List<InventoryItem>();

    public Action OnInventoryChanged;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(ItemsData item, int amount)
    {
        InventoryItem inventoryItem = items.Find(i => i.itemData == item);
        if (inventoryItem != null)
        {
            inventoryItem.amount += amount;
        }
        else
        {
            InventoryItem newItem = new InventoryItem();
            newItem.itemData = item;
            newItem.amount = amount;
            items.Add(newItem);
        }
        
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemsData itemsData, int amount)
    {
        InventoryItem item = items.Find(i => i.itemData == itemsData);
        if (item != null)
        {
            item.amount -= amount;
            if (item.amount <= 0)
            {
                items.Remove(item);
            }
        }
        OnInventoryChanged?.Invoke();
    }

    public int GetAmount(ItemsData type)
    {
        Debug.Log("Gettting amount of " + type.itemName);
        InventoryItem item = items.Find(i => i.itemData == type);
        if (item != null)
        {
            return item.amount;
        }
        return 0;
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemsData itemData;
    public Sprite icon;
    public int amount;

    public InventoryItem()
    {
        itemData = null;
        icon = null;
        amount = 0;
    }
}
