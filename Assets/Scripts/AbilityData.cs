using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    [Header("Text")]
    public string AbilityName;
    [TextArea]
    public string AbilityDescription;
    public string[] TargetTags;

    [Header("Cast Information")]
    public float CastTime;
    public float CastDistance;

    [Header("Visuals")]
    public GameObject[] VFX;

    [Header("Values")]
    public float[] AbilityCosts;
    public float[] AbilityCooldowns;
}
