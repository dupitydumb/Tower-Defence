using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsCard : MonoBehaviour
{
    public bool isQuickSlotCard;
    private PlayerBuild playerBuild;
    private Button button;
    public bool isInventoryCard;
    public bool isCraftingCard;
    private InventoryManager inventoryManager;
    public ItemsData itemData;
    public Image itemIcon;
    private TMP_Text amountText;
    // Start is called before the first frame update
    void Start()
    {
        playerBuild = FindObjectOfType<PlayerBuild>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        amountText = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();

        if (isCraftingCard)
        {
            button.onClick.AddListener(CraftItem);
        }
        else if (isInventoryCard)
        {
            button.onClick.AddListener(PlaceItem);
        }
        else if (isQuickSlotCard && itemData.isPlaceable)
        {
            button.onClick.AddListener(PlaceItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemData(ItemsData item)
    {
        if (inventoryManager == null || amountText == null)
        {
            Debug.Log("Finding inventory manager");
            inventoryManager = FindObjectOfType<InventoryManager>();
            amountText = transform.GetChild(1).GetComponent<TMP_Text>();
        }
        itemData = item;
        itemIcon.sprite = itemData.itemIcon;
        transform.GetChild(0).GetComponent<Image>().color = itemData.itemIconColor;
        if(!isCraftingCard)
        {
            Debug.Log("Setting amount text to " + inventoryManager.GetAmount(itemData));
            Debug.Log("Item data is " + itemData.itemName);
            Debug.Log("Inventory manager is " + inventoryManager);
            Debug.LogWarning("Amount text is " + inventoryManager.GetAmount(itemData).ToString());
            amountText.text = inventoryManager.GetAmount(itemData).ToString();
            Debug.Log("Amount text is " + inventoryManager.GetAmount(itemData).ToString());
            if (inventoryManager.GetAmount(itemData) == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        if (isQuickSlotCard && itemData.isPlaceable)
        {
            button.onClick.AddListener(PlaceItem);
        }
        
    }

    public void CraftItem()
    {
        //Check if player has enough resources
        foreach (var recipe in itemData.itemRecipe)
        {
            Debug.Log("Amount of " + recipe.item.itemName + " is " + inventoryManager.GetAmount(recipe.item));
            if (inventoryManager.GetAmount(recipe.item) < recipe.amount)
            {
                Debug.Log("Not enough resources");
                return;
            }
        }
        foreach (var recipe in itemData.itemRecipe)
        {
            inventoryManager.RemoveItem(recipe.item, recipe.amount);
        }
        inventoryManager.AddItem(itemData, 1);
        inventoryManager.OnInventoryChanged?.Invoke();
    }

    public void PlaceItem()
    {
        playerBuild.SetIsPlacing(itemData);
    }
}
