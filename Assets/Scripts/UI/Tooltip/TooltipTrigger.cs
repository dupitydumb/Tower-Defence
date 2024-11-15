using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryManager inventoryManager;
    private Tooltip tooltip;
    public ItemsData item;

    ItemsCard itemsCard;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        tooltip = Tooltip.instance;
        itemsCard = GetComponent<ItemsCard>();
        SetItemData();
        inventoryManager.OnInventoryChanged += SetItemData;
        
    }

    void SetItemData()
    {
        if (itemsCard != null)
        {
            item = itemsCard.itemData;
        }
    }
    //OnPointerEnter is also required to implement IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip_Static(item);
    }

    //OnPointerExit is also required to implement IPointerExitHandler
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip_Static();
    }
}

