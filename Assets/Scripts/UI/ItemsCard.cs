using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsCard : MonoBehaviour
{
    public bool isCraftingCard;
    private InventoryManager inventoryManager;
    public ItemsData itemData;
    public Image itemIcon;
    private TMP_Text amountText;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        amountText = GetComponentInChildren<TMP_Text>();
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
            amountText = GetComponentInChildren<TMP_Text>();
        }
        itemData = item;
        itemIcon.sprite = itemData.itemIcon;
        if(!isCraftingCard)
        {
            Debug.Log("Setting amount text to " + inventoryManager.GetAmount(itemData));
            Debug.Log("Item data is " + itemData.itemName);
            Debug.Log("Inventory manager is " + inventoryManager);
            amountText.text = inventoryManager.GetAmount(itemData).ToString();
            if (inventoryManager.GetAmount(itemData) == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
        
    }
}
