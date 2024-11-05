using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedUI : MonoBehaviour
{
    public InventoryItem item;

    private TMP_Text text;
    private Image icon;
    // Start is called before the first frame update
    void Start()
    {
        SetItem(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(InventoryItem item)
    {
        this.item = item;
        text = gameObject.GetComponentInChildren<TMP_Text>();
        icon = transform.GetChild(0).GetComponent<Image>();

        text.text = item.amount.ToString();
        icon.sprite = item.icon;
    }
}
