using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    public BuildType buildType;
    public string turretPrefab;

    PlayerBuild playerBuild;
    // Start is called before the first frame update
    void Start()
    {
        playerBuild = FindObjectOfType<PlayerBuild>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildClicked()
    {
        Debug.Log("Build clicked");
        playerBuild.SetIsPlacing(turretPrefab, buildType);
    }
}

public enum BuildType
{
    Turret,
    Building
}
