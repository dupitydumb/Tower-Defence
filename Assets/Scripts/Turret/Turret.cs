using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    public TurretData data; // The data of the turret
    public Transform firePoint; // The fire point

    private float fireTimer = 0.0f;

    public GameObject rangeIndicator; // The range indicator
    // Start is called before the first frame update
    void Start()
    {
        rangeIndicator = transform.Find("Range").gameObject;
        rangeIndicator.transform.localScale = new Vector3(0, 0, 1);
        rangeIndicator.transform.localScale = new Vector3(data.range, data.range, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurretFacingEnemy(GetEnemy()))
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= 1.0f / data.fireRate)
            {
                Fire(GetEnemy());
                fireTimer = 0.0f;
            }   
        }
    }

    GameObject GetEnemy()
    {
        // Get all the colliders in the range of the turret
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, data.range);
        // Loop through all the colliders
        foreach (var collider in colliders)
        {
            // Check if the collider has the tag "Enemy"
            if (collider.CompareTag("Enemy"))
            {
                // Return the game object of the collider
                return collider.gameObject;
            }
        }
        // Return null if no enemy is found
        return null;
    }

    //Rotate the turret to face the enemy
    bool isTurretFacingEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            return false;
        }
        Vector3 direction = enemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, rotation, data.rotationSpeed * Time.deltaTime);
        return true;
    }

    void Fire(GameObject enemy)
    {
        GameObject bullet = Instantiate(data.bulletPrefab, firePoint.transform.position, transform.GetChild(0).rotation.normalized);
        // Get the bullet component
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.target = enemy;
        // Set the bullet's speed
        bulletComponent.speed = data.bulletSpeed;
        // Set the bullet's damage
        bulletComponent.damage = data.bulletDamage;
    }
}
