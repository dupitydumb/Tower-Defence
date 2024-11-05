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
        OnInventoryChanged?.Invoke();
    }

    public int GetAmount(ItemsType type)
    {
        InventoryItem item = items.Find(i => i.type == type);
        if (item != null)
        {
            return item.amount;
        }
        else
        {
            return 0;
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public Sprite icon;
    public ItemsType type;
    public int amount;

    public InventoryItem()
    {
        switch (type)
        {
            case ItemsType.Wood:
                icon = Resources.Load<Sprite>("Sprites/wood");
                break;
            case ItemsType.Stone:
                icon = Resources.Load<Sprite>("Sprites/stone");
                break;
            case ItemsType.Iron:
                icon = Resources.Load<Sprite>("Sprites/iron");
                break;
            case ItemsType.Gold:
                icon = Resources.Load<Sprite>("Sprites/gold");
                break;
        }
    }
}
