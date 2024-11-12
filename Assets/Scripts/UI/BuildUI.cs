using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    public TurretData turretData;
    public BuildType buildType;
    public string turretPrefab;

    PlayerBuild playerBuild;
    // Start is called before the first frame update
    void Start()
    {
        playerBuild = FindObjectOfType<PlayerBuild>();
        GameManager.instance.OnDayChange += UpdateUI;
        UpdateUI();
    }

    void UpdateUI()
    {
        Image image = transform.GetChild(0).GetComponent<Image>();
        if (buildType == BuildType.Turret)
        {
            if (turretData.isUnlocked)
            {
                //Change Color
                image.color = Color.white;
            }
            else
            {
                image.color = Color.black;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildClicked()
    {
        if (buildType == BuildType.Turret)
        {
            if (!turretData.isUnlocked)
            {
                Debug.Log("Turret is not unlocked");
                return;
            }
        }
        Debug.Log("Build clicked");
        playerBuild.SetIsPlacing(turretPrefab, buildType);
    }
}

public enum BuildType
{
    Turret,
    Building,
    Tile
}
