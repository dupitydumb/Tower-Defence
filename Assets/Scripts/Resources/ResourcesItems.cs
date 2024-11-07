using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesItems : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip sound;
    public ItemsType type;
    public int amount;
    private InventoryManager inventoryManager;
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        text = GetComponentInChildren<TMP_Text>();
        text.text = amount.ToString();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Instantiate new game object for sound
            GameObject soundObject = new GameObject();
            soundObject.transform.position = transform.position;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
            Destroy(soundObject, sound.length);
            inventoryManager.AddItem(type, amount);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }


}

public enum ItemsType
{
    Wood,
    Stone,
    Iron,
    Gold
}
