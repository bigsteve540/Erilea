using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RESOURCE { None, Mana, Energy, Points }

[CreateAssetMenu(menuName ="Champions/Data Container")]
public class ChampionData : ScriptableObject
{
    [Header("Health")]
    public float BaseHealth;
    public float HealthPerLevel;

    public float BaseHealthRegen;
    public float HealthRegenPerLevel;

    [Header("Resource")]
    public RESOURCE ResourceType;

    public float BaseResource;
    public float ResourcePerLevel;

    public float BaseResourceRegen;
    public float ResourceRegenPerLevel;

    [Header("Auto Attacking")]
    public float AttackRange;
    public float BaseAttackSpeed;
    public float AttackSpeedPerLevel;

    [Header("Visuals")]
    public GameObject[] VFX;
}
