using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Turret Data", order = 1)]
public class TurretData : ScriptableObject
{
    public GameObject bulletPrefab; // The bullet prefab
    public float fireRate = 1.0f; // The rate of fire
    public float bulletSpeed = 10.0f; // The speed of the bullet
    public float bulletDamage = 10.0f; // The damage of the bullet
    public float range = 5.0f; // The range of the turret
    public float rotationSpeed = 5.0f; // The speed of rotation
    public float health = 100.0f; // The health of the turret
    public float cost = 100.0f; // The cost of the turret
    public Sprite baseSprite; // The base sprite
    public Sprite turretSprite; // The turret sprite
    public Transform firePoint; // The fire point
}
