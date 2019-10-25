using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(StatusController))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(VFXController))]
[RequireComponent(typeof(CCController))]
[RequireComponent(typeof(MovementController))]
public abstract class Entity : MonoBehaviour
{
    protected EntityData stats;
    protected abstract string Filepath { get; }

    [SerializeField]
    public int Level { get; protected set; } = 1;

    [SerializeField]
    private float _adCurrent;
    public float AttackDamage
    {
        get
        {
            return stats.BaseAttackDamage + (stats.ATKDMGPerLevel * (Level - 1));
        }

        protected set
        {
            _adCurrent = value;
        }
    }

    #region Health
    [SerializeField]
    private float _healthCurrent;
    public float Health { get { return _healthCurrent; } protected set { _healthCurrent = Mathf.Clamp(value, 0, MaxHealth); } }
    public float MaxHealth
    {
        get
        {
            return stats.BaseHealth + (stats.HealthPerLevel * (Level - 1));
        }
    }

    public float HealthRegen
    {
        get
        {
            return stats.BaseHealthRegen + (stats.HealthRegenPerLevel * (Level - 1));
        }
    }
    #endregion

    [SerializeField]
    private float _physicalRes = 0;
    public float PhysicalResistance { get { return _physicalRes; } protected set { _physicalRes = value; } }
    [SerializeField]
    private float _magicRes = 0;
    public float MagicResistance { get { return _magicRes; } protected set { _magicRes = value; } }

    private NavMeshAgent Agent { get; set; }

    [SerializeField]
    private float movespeedForAgent = 5f;
    public float MoveSpeed
    {
        get { return movespeedForAgent * 65f; }
        set
        {
            movespeedForAgent = value / 65f;
            Agent.speed = movespeedForAgent;
        }
    }

    protected virtual void OnEnable() { Load<EntityData>(); }
    private void Start() { MoveSpeed = stats.BaseMovementSpeed; }

    public virtual void LevelUp()
    {
        if (Level >= 20)
        { Level = 20; return; }

        Level++;
        Debug.Log("Level: " + Level);

        MaxHealth.ToString();
        Health += stats.HealthPerLevel;

        HealthRegen.ToString();

        AttackDamage.ToString();
    }

    public void AddHealth(float v)
    {
        Health += v;
    }
    public void SubHealth(float v)
    {
        Health -= v;
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

    protected virtual void Load<T>() where T : EntityData
    {
        Agent = GetComponent<NavMeshAgent>();

        stats = Resources.Load<T>(Filepath);

        Health = MaxHealth;
        _adCurrent = AttackDamage;
    }

    public float GetBaseMS()
    {
        return stats.BaseMovementSpeed;
    }

    #region Agent Control
    public virtual void Move(Vector3 position) { Agent.SetDestination(position); }
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
    #endregion


}
