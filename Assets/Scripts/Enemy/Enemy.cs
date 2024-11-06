using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemySpawner enemySpawner;
    public List<GameObject> targetDestination = new List<GameObject>();
    public bool isDummy = false;
    public float health = 100;
    public float speed = 1.0f;
    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        SetDestination();
        enemySpawner = FindObjectOfType<EnemySpawner>();
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

    public void TakeDamage(float damage)
    {
        if (isDummy)
        {
            return;
        }
        health -= damage;
        if (health <= 0)
        {
            enemySpawner.enemiesRemaining--;
            enemySpawner.OnEnemyDeath?.Invoke();
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
