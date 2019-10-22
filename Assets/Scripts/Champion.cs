using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum ABILITY_CODE { Q = 0, W = 1, E = 2, R = 3 }

[RequireComponent(typeof(StatusController))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(AbilityController))]
[RequireComponent(typeof(VFXController))]
public abstract class Champion : MonoBehaviour
{
    public abstract string Name { get; }

    protected ChampionData ChampionStats { get; set; }

    [SerializeField]
    public int Level { get; private set; } = 1;

    [SerializeField]
    private float _shieldNow = 0;
    public float Shield { get { return _shieldNow; } protected set { _shieldNow = value; } }

    #region Health
    [SerializeField]
    private float _healthCurrent;
    public float Health { get { return _healthCurrent; } protected set { _healthCurrent = Mathf.Clamp(value, 0, MaxHealth); } }
    public float MaxHealth
    {
        get
        {
            return ChampionStats.BaseHealth + (ChampionStats.HealthPerLevel * (Level - 1));
        }
    }

    public float HealthRegen
    {
        get
        {
            return ChampionStats.BaseHealthRegen + (ChampionStats.HealthRegenPerLevel * (Level - 1));
        }
    }
    #endregion

    [SerializeField]
    private float _physicalRes = 0;
    public float PhysicalResistance { get { return _physicalRes; } protected set { _physicalRes = value; } }
    [SerializeField]
    private float _magicRes = 0;
    public float MagicResistance { get { return _magicRes; } protected set { _magicRes = value; } }

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
                return ChampionStats.BaseResource + (ChampionStats.ResourcePerLevel * Level);
            }
            return ChampionStats.BaseResource;
        }
    }

    public float ResourceRegen
    {
        get
        {
            return ChampionStats.BaseResourceRegen + (ChampionStats.ResourceRegenPerLevel * (Level - 1));
        }
    }
    #endregion

    public float AttackRange { get { return ChampionStats.AttackRange; } }

    public bool Casting { get; set; }

    [SerializeField]
    private int[] abilityLevels = new int[4] { 1, 1, 1, 1 };

    protected Ability[] abilities;

    private NavMeshAgent Agent { get; set; }

    protected abstract IEnumerator CheckPassiveConditions();

    protected virtual void OnEnable() { LoadChampion(Name); LoadAbilities(); }

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

    public void AddHealth(float v)
    {
        Health += v;
    }
    public void SubHealth(float v)
    {
        Health -= v;
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

    public void AddResistances(DAMAGE_TYPE type, float val)
    {
        switch (type)
        {
            case DAMAGE_TYPE.Mixed:
                PhysicalResistance += val;
                MagicResistance += val;
                break;
            case DAMAGE_TYPE.Magical:
                MagicResistance += val;
                break;
            case DAMAGE_TYPE.Physical:
                PhysicalResistance += val;
                break;
            default:
                throw new Exception($"Cannot Add resistance for: {type} damage");
        }
    }
    public void RemoveResistances(DAMAGE_TYPE type, float val)
    {
        switch (type)
        {
            case DAMAGE_TYPE.Mixed:
                PhysicalResistance -= val;
                MagicResistance -= val;
                break;
            case DAMAGE_TYPE.Magical:
                MagicResistance -= val;
                break;
            case DAMAGE_TYPE.Physical:
                PhysicalResistance -= val;
                break;
            default:
                throw new Exception($"Cannot Remove resistance for: {type} damage");
        }
    }
    public float GetMixedResistances()
    {
        return PhysicalResistance + MagicResistance;
    }

    public void LevelUp()
    {
        if (Level >= 20)
        { Level = 20; return; }

        Level++;
        Debug.Log("Level: " + Level);

        MaxHealth.ToString();
        Health += ChampionStats.HealthPerLevel;

        MaxResource.ToString();
        Resource += ChampionStats.ResourcePerLevel;

        HealthRegen.ToString();

        //Trigger Levelup VFX
    }

    private void LoadChampion(string name)
    {
        Agent = GetComponent<NavMeshAgent>();

        ChampionStats = Resources.Load<ChampionData>($"Champions/{name}/{name}");

        Health = MaxHealth;
        Resource = MaxResource;
    }
    protected abstract void LoadAbilities();

    public void Move(Vector3 position) { if(!Casting) Agent.SetDestination(position); }
    public void CancelPath()
    {
        Agent.isStopped = true;
        Agent.ResetPath();
        Agent.isStopped = false;
    }
    public void StopMoving()
    {
        Agent.isStopped = true;
    }
    public void UnlockMovement()
    {
        Agent.isStopped = false;
    }
    //add TurntoFace method
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
