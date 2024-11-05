using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{

    private GameObject player;

    public float cutTime = 2.0f;
    private float cutTimer = 0.0f;

    private Slider progressBar;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        progressBar = gameObject.GetComponentInChildren<Slider>();
        progressBar.maxValue = cutTime;
        progressBar.value = cutTimer;
    }

    // Update is called once per frame
    void Update()
    {
        CutTree();
    }


    void CutTree()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 2.0f)
        {
            progressBar.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.R))
            {
                //Debug.Log("Cutting tree");
                cutTimer += Time.deltaTime;
                progressBar.value = cutTimer;
                if (cutTimer >= cutTime)
                {
                    SpawnWood();
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            progressBar.gameObject.SetActive(false);
            cutTimer = 0.0f; // Reset the timer if the player moves away
            progressBar.value = cutTimer; // Reset the progress bar
        }

    }

    void SpawnWood()
    {
        GameObject wood = Instantiate(Resources.Load("Prefabs/Items/Wood"), transform.position, Quaternion.identity) as GameObject;
        wood.GetComponent<ResourcesItems>().amount = Random.Range(10, 25);

    }
}
