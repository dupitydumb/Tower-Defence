using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMining : MonoBehaviour, IRecipe
{

    public RecipeData[] itemsRecipe;
    public float craftTime;
    public RecipeData[] ItemsRecipe
    {
        get => itemsRecipe;
        set => itemsRecipe = value;
    }
    public float CraftTime 
    {
        get => craftTime;
        set => craftTime = value;
    }
    // Start is called before the first frame update
    public bool isEnable = false;
    public ItemsData type;
    public int health = 100;
    public int maxHealth = 100;
    public int miningRate = 10;
    public float modifier = 1.0f;    
    public List<InventoryItem> itemNeeded = new List<InventoryItem>();
    private InventoryManager inventoryManager;
    private float miningTimer = 0.0f;
    private GameObject canvasUI;
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        canvasUI = gameObject.transform.Find("Canvas").gameObject;
        SetNeededItemUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnable)
        Mine();
    }

    void SetNeededItemUI()
    {
        Debug.Log("Setting needed item UI");
        foreach (var item in itemNeeded)
        {
            GameObject itemUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/NeedUI"), canvasUI.transform);
            itemUI.GetComponent<NeedUI>().item = item;
        }
    }
    void Mine()
    {
        miningTimer += Time.deltaTime;
        if (miningTimer >= 1.0f / miningRate * modifier)
        {
            miningTimer = 0.0f;
            inventoryManager.AddItem(type, 1);
        }
    }
}
