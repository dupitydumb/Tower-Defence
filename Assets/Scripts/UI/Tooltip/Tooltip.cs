using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public static Tooltip instance;
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Follow the mouse
        transform.position = Input.mousePosition;
    }

    public void SetItemData(ItemsData item)
    {
        if (item == null)
        {
            return;
        }
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;
    }

    public void ShowTooltip_Static(ItemsData item)
    {
        SetItemData(item);
        gameObject.SetActive(true);
    }

    public void HideTooltip_Static()
    {
        gameObject.SetActive(false);
    }
}
