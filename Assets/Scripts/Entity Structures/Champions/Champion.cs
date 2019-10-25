using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum ABILITY_CODE { Q = 0, W = 1, E = 2, R = 3 }

[RequireComponent(typeof(AbilityController))]
public abstract class Champion : Entity
{
    public abstract string Name { get; }

    [SerializeField]
    private float _shieldNow = 0;
    public float Shield { get { return _shieldNow; } protected set { _shieldNow = value; } }
   
    #region Resources
    [SerializeField]
    private float _resourceCurrent;
    public float Resource { get { return _resourceCurrent; } protected set { _resourceCurrent = Mathf.Clamp(value, 0, MaxResource); } }
    public float MaxResource
    {
        get
        {
            if (Level > 1)
            {
                return (stats as ChampionData).BaseResource + ((stats as ChampionData).ResourcePerLevel * Level);
            }
            return (stats as ChampionData).BaseResource;
        }
    }

    public float ResourceRegen
    {
        get
        {
            return (stats as ChampionData).BaseResourceRegen + ((stats as ChampionData).ResourceRegenPerLevel * (Level - 1));
        }
    }
    #endregion

    public float AttackRange { get { return stats.AttackRange; } }

    public bool Casting { get; set; }

    [SerializeField]
    private int[] abilityLevels = new int[4] { 1, 1, 1, 1 };

    protected Ability[] abilities;

    protected abstract IEnumerator CheckPassiveConditions();

    protected override void OnEnable() { Load<ChampionData>(); LoadAbilities(); }

    public int GetAbilityLevel(int index)
    {
        return abilityLevels[index];
    }
    public Ability GetAbility(int index)
    {
        return abilities[index];
    }
    public AbilityData GetAbilityData(int index)
    {
        return abilities[index].GetData();
    }

    public void AddShield(float v)
    {
        Shield += v;
    }
    public void SubShield(float v)
    {
        Shield -= v;
    }
    public void NukeShield()
    {
        Shield = 0;
    }

    public override void LevelUp()
    {
        if (Level >= 20)
        { Level = 20; return; }

        base.LevelUp();

        MaxResource.ToString();
        Resource += (stats as ChampionData).ResourcePerLevel;
        //Trigger Levelup VFX
    }

    protected override void Load<T>()
    {
        base.Load<T>();

        Resource = MaxResource;
    }
    protected abstract void LoadAbilities();
    public override void Move(Vector3 position) { if (!Casting) base.Move(position); }
}

public enum EFFECTOR_TYPE { Flat, PercentMax, PercentMis, PercentCurrent }
public enum DAMAGE_TYPE { Physical, Magical, Mixed, True }
[Serializable]
public struct DamageData
{
    [HideInInspector]
    public GameObject Dealer;
    [HideInInspector]
    public GameObject Receiver;
    public float Value;
    public EFFECTOR_TYPE Type;
    public DAMAGE_TYPE DamageType;

    public DamageData(GameObject dealer, GameObject receiver, float val, EFFECTOR_TYPE type, DAMAGE_TYPE dtype)
    {
        Dealer = dealer;
        Receiver = receiver;
        Value = val;
        Type = type;
        DamageType = dtype;
    }
}
[Serializable]
public struct HealData
{
    [HideInInspector]
    public GameObject Dealer;
    [HideInInspector]
    public GameObject Receiver;
    public float Value;
    public EFFECTOR_TYPE Type;

    public HealData(GameObject dealer, GameObject receiver, float val, EFFECTOR_TYPE type)
    {
        Dealer = dealer;
        Receiver = receiver;
        Value = val;
        Type = type;
    }
}

[Serializable]
public struct ResourceEffector
{
    public bool IsNegative;
    public float Value;
    public EFFECTOR_TYPE Type;

    public ResourceEffector(bool isNeg, float val, EFFECTOR_TYPE type)
    {
        IsNegative = isNeg;
        Value = val;
        Type = type;
    }
}
