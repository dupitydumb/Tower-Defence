using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
    [SerializeField]
    public bool isEnable = false;
    public TurretData data; // The data of the turret
    public Transform firePoint; // The fire point
    private float fireTimer = 0.0f;
    public GameObject rangeIndicator; // The range indicator
    bool isSelected = false;
    private AudioSource audioSource;
    public List<InventoryItem> itemNeeded = new List<InventoryItem>();
    private GameObject canvasUI;
    private UpgradeBuildingUI upgradeBuildingUI;
    // Start is called before the first frame update
    void Start()
    {
        data = data.Clone();
        rangeIndicator = transform.Find("Range").gameObject;
        rangeIndicator.transform.localScale = new Vector3(0, 0, 1);
        rangeIndicator.transform.localScale = new Vector3(data.range[data.rangeLevel - 1] * 2.5f, data.range[data.rangeLevel - 1] * 2.5f, 1);
        canvasUI = gameObject.transform.Find("Canvas").gameObject;
        upgradeBuildingUI = GameManager.instance.upgradeBuildingUI;
        audioSource = GetComponent<AudioSource>();
        SetNeededItemUI();
        audioSource.clip = data.shootSound;
    }


    void SetNeededItemUI()
    {
        Debug.Log("Setting needed item UI");
        foreach (var item in itemNeeded)
        {
            GameObject itemUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/NeedUI"), canvasUI.transform);
            itemUI.GetComponent<NeedUI>().item = item;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // If the turret is not enabled, return
        if (!isEnable)
        {
            return;
        }
        if (isTurretFacingEnemy(GetEnemy()))
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= 1.0f / data.fireRate[data.fireRateLevel - 1])
            {
                Fire(GetEnemy());
                fireTimer = 0.0f;
            }   
        }


        //If mouse is over the turret, show the range indicator

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            isSelected = false;
        }

        if (isSelected)
        {
            rangeIndicator.SetActive(true);
        }
        else
        {
            rangeIndicator.SetActive(false);
        }
    }



    void OnMouseOver()
    {
        Debug.Log("Mouse is over the turret");
        isSelected = true;
        rangeIndicator.transform.localScale = new Vector3(data.range[data.rangeLevel - 1] * 2.5f, data.range[data.rangeLevel - 1] * 2.5f, 1);
        if (Input.GetMouseButtonDown(0))
        {
            upgradeBuildingUI.SetSelectedTurret(this);
        }
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse is not over the turret");
        isSelected = false;
    }
    GameObject GetEnemy()
    {
        // Get all the colliders in the range of the turret
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, data.range[data.rangeLevel - 1]);
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
        bulletComponent.speed = data.bulletSpeed[data.bulletSpeedLevel - 1];
        // Set the bullet's damage
        bulletComponent.damage = data.bulletDamage[data.bulletDamageLevel - 1];
        // Play the shoot sound
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
