using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Data")]
public class ItemsData : ScriptableObject
{
    public GameObject itemPrefab;
    public Sprite itemIcon;
    public string itemName;
    public string itemDescription;
    public InventoryItem[] itemRecipe;
    public float itemBuildTime;

}
