using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : Controller
{
    public delegate void DamageTaken(DamageData dd);
    public delegate void HealTaken(HealData hd);
    public delegate void OnDying();

    public static event HealTaken OnHealTaken;
    public static event DamageTaken OnDamageTaken;
    public static event OnDying OnDeath;

    protected override void Start() { base.Start(); InvokeRepeating("RegenerateHP", 1f, 1f); }
    protected virtual void RegenerateHP()
    {
        target.AddHealth(target.HealthRegen);
    }

    public virtual void TakeDamage(DamageData dd)
    {
        float damage = 0f;

        switch (dd.DamageType)
        {
            case DAMAGE_TYPE.Mixed:
                damage = dd.Value * 1 - target.GetMixedResistances();
                break;
            case DAMAGE_TYPE.Magical:
                damage = dd.Value * 1 - target.MagicResistance;
                break;
            case DAMAGE_TYPE.Physical:
                damage = dd.Value * 1 - target.PhysicalResistance;
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
                    target.SubHealth(damage);
                }
                break;
            case EFFECTOR_TYPE.PercentCurrent:
                {
                    damage = ShieldDamage(target.Health * dd.Value);
                    target.SubHealth(damage);
                }
                break;
            case EFFECTOR_TYPE.PercentMax:
                {
                    damage = ShieldDamage(target.MaxHealth * dd.Value);
                    target.SubHealth(damage);
                }
                break;
            case EFFECTOR_TYPE.PercentMis:
                {
                    damage = ShieldDamage((target.MaxHealth - target.Health) * dd.Value);
                    target.SubHealth(damage);
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
                    target.AddHealth(heal);
                }
                break;
            case EFFECTOR_TYPE.PercentCurrent:
                {
                    heal = target.Health * hd.Value;
                    target.AddHealth(heal);
                }
                break;
            case EFFECTOR_TYPE.PercentMax:
                {
                    heal = target.MaxHealth * hd.Value;
                    target.AddHealth(heal);
                }
                break;
            case EFFECTOR_TYPE.PercentMis:
                {
                    heal = (target.MaxHealth - target.Health) * hd.Value;
                    target.AddHealth(heal);
                }
                break;
        }
        HealData healTaken = new HealData(hd.Dealer, gameObject, heal, hd.Type);
        OnHealTaken?.Invoke(healTaken);
    }

    private float ShieldDamage(float value)
    {
        if (!(target is Champion))
            return value;

        Champion c = target as Champion;

        if (c.Shield >= value)
        {
            c.SubShield(value);
            return 0f;
        }
        else
        {
            value -= c.Shield;
            c.NukeShield();
            return value;
        }
    }

    private void CheckForDeath()
    {
        if (target.Health <= 0f)
        {
            OnDeath?.Invoke();
        }
    }
}
