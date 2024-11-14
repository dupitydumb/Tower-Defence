using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject playerInventoryPanel;
    public GameObject craftingPanel;

    private InventoryManager inventoryManager;

    public GameObject[] quickSlotPanel;

    [SerializeField]
    private List<ItemsData> itemsData = new List<ItemsData>();
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        InitializedCraftingPanel();
        menuUI.SetActive(false);
        InitializedQuickSlotPanel();
        inventoryManager.OnInventoryChanged += InitializedQuickSlotPanel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleMenu();
        }
    }

    void OnEnable()
    {
        GetItemsData();
    }

    public void ToggleMenu()
    {
        menuUI.SetActive(!menuUI.activeSelf);
        if (menuUI.activeSelf)
        {
            InitializedInventoryPanel();
        }
    }
    void GetItemsData()
    {
        // Loop through all ItemsData folder in resources
        foreach (ItemsData item in Resources.LoadAll<ItemsData>("ItemsData"))
        {
            if (!itemsData.Contains(item))
            itemsData.Add(item);
        }
    }
    void InitializedCraftingPanel()
    {
        Debug.Log("Initialized Crafting Panel");
        // Loop through all ItemsData folder in resources
        foreach (ItemsData item in itemsData)
        {
            if (item.isCraftable)
            {
                GameObject itemCard = Instantiate(Resources.Load("Prefabs/UI/ItemsCard", typeof(GameObject))) as GameObject;
                itemCard.transform.SetParent(craftingPanel.transform);
                itemCard.GetComponent<ItemsCard>().isCraftingCard = true;
                itemCard.GetComponent<ItemsCard>().SetItemData(item);
            }
        }
    }

    void InitializedQuickSlotPanel()
    {
        Debug.Log("Initialized Quick Slot Panel");
        // Loop through all ItemsData folder in resources
        for (int i = 0; i < inventoryManager.items.Count; i++)
        {
            quickSlotPanel[i].GetComponent<ItemsCard>().SetItemData(inventoryManager.items[i].itemData);
        }

        for (int i = 0; i < quickSlotPanel.Length; i++)
        {
            if (quickSlotPanel[i].GetComponent<ItemsCard>().itemData == null)
            {
                quickSlotPanel[i].transform.GetChild(0).gameObject.SetActive(false);
                quickSlotPanel[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                quickSlotPanel[i].transform.GetChild(0).gameObject.SetActive(true);
                quickSlotPanel[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        
    }

    void InitializedInventoryPanel()
    {
        Debug.Log("Initialized Inventory Panel");
        // Loop through all ItemsData folder in resources
        for (int i = 0; i < playerInventoryPanel.transform.childCount; i++)
        {
            Destroy(playerInventoryPanel.transform.GetChild(i).gameObject);
        }
        foreach (InventoryItem item in inventoryManager.items)
        {
            GameObject itemCard = Instantiate(Resources.Load("Prefabs/UI/ItemsCard", typeof(GameObject))) as GameObject;
            itemCard.transform.SetParent(playerInventoryPanel.transform);
            itemCard.GetComponent<ItemsCard>().SetItemData(item.itemData);
        }
    }
}
