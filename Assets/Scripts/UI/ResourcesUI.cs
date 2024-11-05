using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public ItemsType type;
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        text = gameObject.GetComponentInChildren<TMP_Text>();
        UpdateText();
        inventoryManager.OnInventoryChanged += UpdateText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateText()
    {
        text.text = inventoryManager.GetAmount(type).ToString();
    }
}
