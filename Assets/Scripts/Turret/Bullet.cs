using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject target;
    public float speed = 10.0f;
    public float damage;

    public AudioClip hitSound;
    public GameObject explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Face the target
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            transform.position += direction.normalized * speed * Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<Enemy>().TakeDamage(damage);
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, collider.transform.position, Quaternion.identity);
                GameObject soundObject = new GameObject();
                soundObject.transform.position = transform.position;
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();
                audioSource.clip = hitSound;
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
                Destroy(soundObject, hitSound.length);
            }
            Destroy(gameObject);
        }
    }
}
