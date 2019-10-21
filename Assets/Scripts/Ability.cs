using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [Header("Text")]
    public string AbilityName;
    [TextArea]
    public string AbilityDescription;
    public string[] TargetTags;

    [Header("Cast Information")]
    public float CastTime;
    public float CastDistance;

    [Header("Values")]
    public ValueEffector[] AbilityValues;
    public float[] AbilityCosts;
    public float[] AbilityCooldowns;

    public abstract void Fire(GameObject caster, int abilityLevel);
}
