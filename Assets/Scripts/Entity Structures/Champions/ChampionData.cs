using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RESOURCE { None, Mana, Energy, Points }

[CreateAssetMenu(menuName ="Champions/Data")]
public class ChampionData : EntityData
{
    [Header("Resource")]
    public RESOURCE ResourceType;

    public float BaseResource;
    public float ResourcePerLevel;

    public float BaseResourceRegen;
    public float ResourceRegenPerLevel;
}
