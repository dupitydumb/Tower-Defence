using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunResearchCard : MonoBehaviour
{
    public TurretData turretData;
    private InventoryManager inventoryManager;
    public TMP_Text price;
    public TMP_Text turretName;
    public Image turretImage;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        price.text = turretData.unlockCost.ToString();
        turretName.text = turretData.turretName;
        turretImage.sprite = turretData.turretSprite;
        if (turretData.isResearched)
        {
            price.text = "Researched";
        }

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(UnlockGun);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockGun()
    {
        //Search gold in inventory
        int goldNeeded = turretData.unlockCost;
        int goldAmount = inventoryManager.GetAmount(ItemsType.Gold);
        if (goldAmount >= goldNeeded)
        {
            inventoryManager.RemoveItem(ItemsType.Gold, goldNeeded);
            //Unlock Next day
            GameManager.instance.AddResearch(turretData);
            price.text = "Researched";
        }
    }
}
