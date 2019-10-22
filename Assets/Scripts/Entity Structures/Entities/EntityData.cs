using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Data")]
public class EntityData : ScriptableObject
{
    [Header("Health")]
    public float BaseHealth;
    public float HealthPerLevel;

    public float BaseHealthRegen;
    public float HealthRegenPerLevel;

    [Header("Auto Attacking")]
    public float AttackRange;
    public float BaseAttackSpeed;
    public float AttackSpeedPerLevel;

    [Header("Visuals")]
    public GameObject[] VFX;

    [Header("Movement")]
    public float BaseMovementSpeed;
}
