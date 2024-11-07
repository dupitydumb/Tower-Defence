using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTable : MonoBehaviour
{

    private GameObject player;
    private GameObject researchUI;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        researchUI = GameManager.instance.researchUI;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 1f)
        {
            OpenWindow();
        }
        else
        {
            researchUI.gameObject.SetActive(false);
        }
    }

    void OpenWindow()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            researchUI.gameObject.SetActive(true);
        }
    }
}
