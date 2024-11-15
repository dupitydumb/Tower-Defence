using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Data")]
public class ItemsData : ScriptableObject
{
    public itemCategory category;
    public GameObject itemPrefab;
    public Sprite itemIcon;
    public Color itemIconColor = Color.white;
    public string itemName;
    public string itemDescription;
    public RecipeData[] itemRecipe;
    public float itemBuildTime;
    public bool isCraftable;
    public bool isUnlocked;
    public bool isPlaceable;
}
[System.Serializable]
public enum itemCategory
{
    Resource,
    Building,
    Turret,
    Tile
}
