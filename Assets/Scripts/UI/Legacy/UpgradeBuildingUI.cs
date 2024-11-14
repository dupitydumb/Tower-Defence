using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBuildingUI : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public Turret selectedTurret;

    public GameObject damageUI;
    public GameObject fireRateUI;
    public GameObject rangeUI;
    public GameObject upgradeDamageButton;
    public GameObject upgradeFireRateButton;
    public GameObject upgradeRangeButton;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.upgradeBuildingUI = this;
        inventoryManager = FindObjectOfType<InventoryManager>();
        upgradeDamageButton.GetComponent<Button>().onClick.AddListener(UpgradeDamage);
        upgradeFireRateButton.GetComponent<Button>().onClick.AddListener(UpgradeFireRate);
        upgradeRangeButton.GetComponent<Button>().onClick.AddListener(UpgradeRange);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetSelectedTurret(Turret turret)
    {
        selectedTurret = turret;
        UpdateUI();
        this.gameObject.SetActive(true);
    }

    void UpdateUI()
    {
        damageUI.GetComponentInChildren<TMP_Text>().text = selectedTurret.data.bulletDamage[selectedTurret.data.bulletDamageLevel - 1].ToString();
        fireRateUI.GetComponentInChildren<TMP_Text>().text = selectedTurret.data.fireRate[selectedTurret.data.fireRateLevel - 1].ToString();
        rangeUI.GetComponentInChildren<TMP_Text>().text = selectedTurret.data.range[selectedTurret.data.rangeLevel - 1].ToString();

        TMP_Text upgradeDamageText = upgradeDamageButton.transform.Find("Price").GetComponent<TMP_Text>();
        upgradeDamageText.text = selectedTurret.data.upgradeCost[selectedTurret.data.bulletDamageLevel - 1].ToString();
        TMP_Text upgradeFireRateText = upgradeFireRateButton.transform.Find("Price").GetComponent<TMP_Text>();
        upgradeFireRateText.text = selectedTurret.data.upgradeCost[selectedTurret.data.fireRateLevel - 1].ToString();
        TMP_Text upgradeRangeText = upgradeRangeButton.transform.Find("Price").GetComponent<TMP_Text>();
        upgradeRangeText.text = selectedTurret.data.upgradeCost[selectedTurret.data.rangeLevel - 1].ToString();

        if (selectedTurret.data.bulletDamageLevel >= selectedTurret.data.bulletDamage.Length)
        {
            upgradeDamageButton.GetComponentInChildren<TMP_Text>().text = "Max";
        }
        if (selectedTurret.data.fireRateLevel >= selectedTurret.data.fireRate.Length)
        {
            upgradeFireRateButton.GetComponentInChildren<TMP_Text>().text = "Max";
        }
        if (selectedTurret.data.rangeLevel >= selectedTurret.data.range.Length)
        {
            upgradeRangeButton.GetComponentInChildren<TMP_Text>().text = "Max";
        }

    }

    void UpgradeDamage()
    {
        if (selectedTurret.data.bulletDamageLevel >= selectedTurret.data.bulletDamage.Length)
        {
            return;
        }
        //Find amount of gold needed to upgrade
        int amountNeeded = selectedTurret.data.upgradeCost[selectedTurret.data.bulletDamageLevel - 1];
        //Check if player has enough gold

    }

    void UpgradeFireRate()
    {
        if (selectedTurret.data.fireRateLevel >= selectedTurret.data.fireRate.Length)
        {
            return;
        }
        //Find amount of gold needed to upgrade
        int amountNeeded = selectedTurret.data.upgradeCost[selectedTurret.data.fireRateLevel - 1];
        //Check if player has enough gold
    }

    void UpgradeRange()
    {
        if (selectedTurret.data.rangeLevel >= selectedTurret.data.range.Length)
        {
            return;
        }
        //Find amount of gold needed to upgrade
        int amountNeeded = selectedTurret.data.upgradeCost[selectedTurret.data.rangeLevel - 1];
        //Check if player has enough gold
    }
}
