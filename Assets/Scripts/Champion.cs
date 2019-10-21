using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ABILITY_CODE { Q = 0, W = 1, E = 2, R = 3 }

public abstract class Champion : MonoBehaviour
{
    public virtual string Name { get; }

    protected ChampionData ChampionStats { get; set; }

    public Transform VFXPoint;

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
            if (Level > 1)
            {
                return ChampionStats.BaseHealth + (ChampionStats.HealthPerLevel * Level);
            }
            return ChampionStats.BaseHealth;
        }
    }
    #endregion

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
    #endregion

    public float AttackRange { get { return ChampionStats.AttackRange; } }

    [SerializeField]
    private int[] abilityLevels = new int[4] { 1, 1, 1, 1 };

    private NavMeshAgent agent;

    public delegate void HealthModified(float v);

    protected event HealthModified OnHealTaken;
    protected event HealthModified OnDamageTaken;

    protected abstract IEnumerator CheckPassiveConditions();

    private void LoadChampion(string name)
    {
        agent = GetComponent<NavMeshAgent>();

        ChampionStats = Resources.Load<ChampionData>($"Champions/{name}/{name}");

        Health = MaxHealth;
        Resource = MaxResource;
    }

    public virtual void ModifyHealth(ValueEffector v)
    {
        if (v.IsNegative)
        {
            float damage = 0f;
            switch (v.Type)
            {
                case EFFECTOR_TYPE.Flat:
                    {
                        damage = ShieldDamage(v.Value);
                        Health -= damage;
                    }
                    break;
                case EFFECTOR_TYPE.PercentCurrent:
                    {
                        damage = ShieldDamage(Health * v.Value);
                        Health -= damage;
                    }
                    break;
                case EFFECTOR_TYPE.PercentMax:
                    {
                        damage = ShieldDamage(MaxHealth * v.Value);
                        Health -= damage;
                    }
                    break;
                case EFFECTOR_TYPE.PercentMis:
                    {
                        damage = ShieldDamage((MaxHealth - Health) * v.Value);
                        Health -= damage;
                    }
                    break;
            }
            CheckForDeath();
            OnDamageTaken?.Invoke(damage);
        }
        else
        {
            float heal = 0f;
            switch (v.Type)
            {
                case EFFECTOR_TYPE.Flat:
                    {
                        heal = v.Value;
                        _healthCurrent += heal;
                    }
                    break;
                case EFFECTOR_TYPE.PercentCurrent:
                    {

                    }
                    break;
                case EFFECTOR_TYPE.PercentMax:
                    {

                    }
                    break;
                case EFFECTOR_TYPE.PercentMis:
                    {

                    }
                    break;
            }
            OnHealTaken?.Invoke(heal);
        }
    }

    private void CheckForDeath()
    {
        if(_healthCurrent <= 0f)
        {
            //do death shit
        }
    }

    private float ShieldDamage(float value)
    {
        if(_shieldNow >= value)
        {
            _shieldNow -= value;
            return 0f;
        }
        else
        {
            value -= _shieldNow;
            _shieldNow = 0;
            return value;
        }
    }

    public void Cast(ABILITY_CODE key) { ChampionStats.Abilities[System.Convert.ToInt32(key)].Fire(gameObject, 1); }

    public void Move(Vector3 position) { agent.isStopped = false; agent.SetDestination(position); }

    public void StopMoving() { agent.isStopped = true; }

    protected virtual void Awake() { LoadChampion(Name); }

    void Update()
    {
        
    }
}

public enum EFFECTOR_TYPE { Flat, PercentMax, PercentMis, PercentCurrent }
[System.Serializable]
public struct ValueEffector
{
    public bool IsNegative;
    public float Value;
    public EFFECTOR_TYPE Type;

    public ValueEffector(bool isNeg, float val, EFFECTOR_TYPE type)
    {
        Value = val;
        IsNegative = isNeg;
        Type = type;
    }
}
