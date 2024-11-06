using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchItems", menuName = "Research/ResearchItems")]
public class ResearchItems : ScriptableObject
{
    public BuffType buffType;
    public float modifier;
    public float cost;
    public string names;
    public string description;
    public bool isOnProgress;
    public bool isResearched;
    public bool isUnlocked;
}


public enum BuffType
{
    AttackSpeed,
    Damage,
    MinerSpeed,
}
