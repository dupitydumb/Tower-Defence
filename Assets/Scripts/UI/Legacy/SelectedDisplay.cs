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
}
