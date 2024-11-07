using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip hitSound;
    public Direction direction;
    public Sprite[] bodySprites;
    EnemySpawner enemySpawner;
    public List<GameObject> targetDestination = new List<GameObject>();
    public bool isDummy = false;
    public float health = 100;
    public float speed = 1.0f;
    public int damage = 10;
    private Vector2 movement;
    private Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        SetDestination();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;
        audioSource = GetComponent<AudioSource>();
    }


    void SetDestination()
    {
        GameObject target = GameObject.Find("EnemyLine");
        //foreach child in targetDestination add to list
        foreach (Transform child in target.transform)
        {
            //skip child number 0
            if (child.GetSiblingIndex() == 0)
            {
                continue;
            }
            targetDestination.Add(child.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        movement.y = transform.position.y - targetDestination[currentDestinationIndex].transform.position.y;
        movement.x = transform.position.x - targetDestination[currentDestinationIndex].transform.position.x;
        
    }
    private int currentDestinationIndex = 0;
    void Move()
    {
        if (targetDestination.Count == 0)
        {
            return;
        }
        Vector3 direction = targetDestination[currentDestinationIndex].transform.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetDestination[currentDestinationIndex].transform.position) < 0.1f)
        {
            currentDestinationIndex++;
            if (currentDestinationIndex >= targetDestination.Count)
            {
                Destroy(gameObject);
            }
        }

    }

    public void SetPlayerSprites()
    {
        SpriteRenderer headSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer bodySpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();

        headSpriteRenderer.flipX = false;
        bodySpriteRenderer.flipX = false;
        switch (GetDirection())
        {
            case Direction.North:

                bodySpriteRenderer.sprite = bodySprites[1];
                break;
            case Direction.East:

                bodySpriteRenderer.sprite = bodySprites[2];
                break;
            case Direction.South:

                bodySpriteRenderer.sprite = bodySprites[0];
                break;
            case Direction.West:
                bodySpriteRenderer.sprite = bodySprites[2];
                headSpriteRenderer.flipX = true;
                bodySpriteRenderer.flipX = true;
                break;
        }
    }

    Direction GetDirection()
    {
        if (movement.x > 0)
        {
            direction = Direction.East;
        }
        else if (movement.x < 0)
        {
            direction = Direction.West;
        }
        else if (movement.y > 0)
        {
            direction = Direction.North;
        }
        else if (movement.y < 0)
        {
            direction = Direction.South;
        }
        return direction;
    }

    public void UpdateHealthBar()
    {
        healthBar.value = health;
    }
    public void TakeDamage(float damage)
    {
        if (isDummy)
        {
            return;
        }
        health -= damage;
        audioSource.PlayOneShot(hitSound);
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        UpdateHealthBar();
        if (health <= 0)
        {
            enemySpawner.enemiesRemaining--;
            enemySpawner.OnEnemyDeath?.Invoke();
            Die();
        }
    }

    void Die()
    {
        audioSource.PlayOneShot(deathSound);
        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Reactor"))
        {
            GameManager.instance.ShowGameOver();
            Die();
        }
        
    }
}
