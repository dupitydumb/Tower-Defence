using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Turret Data", order = 1)]
public class TurretData : ScriptableObject
{
    public int fireRateLevel = 1; // The level of the fire rate
    public int bulletSpeedLevel = 1; // The level of the bullet speed
    public int bulletDamageLevel = 1; // The level of the bullet damage
    public int rangeLevel = 1; // The level of the range
    public GameObject bulletPrefab; // The bullet prefab
    public int[] upgradeCost; // The cost of upgrading the turret
    public float[] fireRate;
    public float[] bulletSpeed;
    public float[] bulletDamage;
    public float[] range;
    public float rotationSpeed = 5.0f; // The speed of rotation
    public float health = 100.0f; // The health of the turret
    public float cost = 100.0f; // The cost of the turret
    public Sprite baseSprite; // The base sprite
    public Sprite turretSprite; // The turret sprite
    public Transform firePoint; // The fire point
}
