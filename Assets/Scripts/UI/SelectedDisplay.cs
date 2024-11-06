using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedDisplay : MonoBehaviour
{
    public ResearchItems researchItem;
    public TMP_Text names;
    public TMP_Text description;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        
        if (researchItem == null)
        {
            return;
        }
        names.text = researchItem.names;
        description.text = researchItem.description;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDisplay(ResearchItems item)
    {
        Debug.Log("Setting display items : " + item.names);
        researchItem = item;
        names.text = researchItem.names;
        description.text = researchItem.description;
        button.onClick.AddListener(() => Research(researchItem));
    }

    public void Research(ResearchItems items)
    {
        GameManager.instance.AddResearch(items);
    }
}
