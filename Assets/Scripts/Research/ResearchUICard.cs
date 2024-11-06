using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchUICard : MonoBehaviour
{
    private SelectedDisplay selectedDisplay;
    private Button button;
    public ResearchItems researchItem;
    private TMP_Text nameText;
    private Slider progressSlider;
    // Start is called before the first frame update
    void Start()
    {
        selectedDisplay = FindObjectOfType<SelectedDisplay>();
        nameText = GetComponentInChildren<TMP_Text>();
        nameText.text = researchItem.names;
        button = GetComponent<Button>();
        progressSlider = GetComponentInChildren<Slider>();
        button.onClick.AddListener(SetDisplay);

        UpdateProgress();
        GameManager.instance.OnResearchAdded += UpdateProgress;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetDisplay()
    {
        selectedDisplay.SetDisplay(researchItem);
    }
    
    void UpdateProgress()
    {

        if (GameManager.instance.researchedItems.Contains(researchItem))
        {
            button.interactable = false;
            progressSlider.value = 1;
        }
        if (researchItem.isOnProgress)
        {
            progressSlider.value = 0.5f;
            progressSlider.gameObject.SetActive(true);
        }
        if (!researchItem.isOnProgress && !GameManager.instance.researchedItems.Contains(researchItem))
        {
            progressSlider.gameObject.SetActive(false);
        }
        //Debug null
        Debug.Log("Getting progress : " + researchItem.names + " : " + researchItem.isOnProgress);
        Debug.Log("Getting progress : " + researchItem.names + " : " + GameManager.instance.researchedItems.Contains(researchItem));
    }

}
