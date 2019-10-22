using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private Champion me;

    public delegate void DamageTaken(DamageData dd);
    public delegate void HealTaken(HealData hd);
    public delegate void OnDying();

    public static event HealTaken OnHealTaken;
    public static event DamageTaken OnDamageTaken;
    public static event OnDying OnDeath;

    void Start() { InvokeRepeating("RegenerateHP", 1f, 1f); me = GetComponent<Champion>(); }
    protected virtual void RegenerateHP()
    {
        me.AddHealth(me.HealthRegen);
    }

    public virtual void TakeDamage(DamageData dd)
    {
        float damage = 0f;

        switch (dd.DamageType)
        {
            case DAMAGE_TYPE.Mixed:
                damage = dd.Value * 1 - me.GetMixedResistances();
                break;
            case DAMAGE_TYPE.Magical:
                damage = dd.Value * 1 - me.MagicResistance;
                break;
            case DAMAGE_TYPE.Physical:
                damage = dd.Value * 1 - me.PhysicalResistance;
                break;
            case DAMAGE_TYPE.True:
                damage = dd.Value;
                break;
        }

        switch (dd.Type)
        {
            case EFFECTOR_TYPE.Flat:
                {
                    damage = ShieldDamage(dd.Value);
                    me.SubHealth(damage);
                }
                break;
            case EFFECTOR_TYPE.PercentCurrent:
                {
                    damage = ShieldDamage(me.Health * dd.Value);
                    me.SubHealth(damage);
                }
                break;
            case EFFECTOR_TYPE.PercentMax:
                {
                    damage = ShieldDamage(me.MaxHealth * dd.Value);
                    me.SubHealth(damage);
                }
                break;
            case EFFECTOR_TYPE.PercentMis:
                {
                    damage = ShieldDamage((me.MaxHealth - me.Health) * dd.Value);
                    me.SubHealth(damage);
                }
                break;
        }
        CheckForDeath();

        DamageData damageTaken = new DamageData(dd.Dealer, gameObject, damage, dd.Type, dd.DamageType);
        OnDamageTaken?.Invoke(damageTaken);
    }

    public virtual void Heal(HealData hd)
    {
        float heal = 0f;
        switch (hd.Type)
        {
            case EFFECTOR_TYPE.Flat:
                {
                    heal = hd.Value;
                    me.AddHealth(heal);
                }
                break;
            case EFFECTOR_TYPE.PercentCurrent:
                {
                    heal = me.Health * hd.Value;
                    me.AddHealth(heal);
                }
                break;
            case EFFECTOR_TYPE.PercentMax:
                {
                    heal = me.MaxHealth * hd.Value;
                    me.AddHealth(heal);
                }
                break;
            case EFFECTOR_TYPE.PercentMis:
                {
                    heal = (me.MaxHealth - me.Health) * hd.Value;
                    me.AddHealth(heal);
                }
                break;
        }
        HealData healTaken = new HealData(hd.Dealer, gameObject, heal, hd.Type);
        OnHealTaken?.Invoke(healTaken);
    }

    private float ShieldDamage(float value)
    {
        if (me.Shield >= value)
        {
            me.SubShield(value);
            return 0f;
        }
        else
        {
            value -= me.Shield;
            me.NukeShield();
            return value;
        }
    }

    private void CheckForDeath()
    {
        if (me.Health <= 0f)
        {
            OnDeath?.Invoke();
        }
    }
}
