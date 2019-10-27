using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuutsuW : Ability
{
    public YuutsuW(Champion c) : base(c, "Champions/Yuutsu/Abilities/Yuutsu W")
    {
        yuutsuWActive = new StatusEffect();

        passive = new FarmersFavour(
            new float[1] { 1f },
            champ.gameObject
            );
    }

    private StatusEffect yuutsuWActive;
    private float[] Durations = new float[5] { 2f, 2.5f, 3f, 3.5f, 4f };
    private float timer;
    private GameObject activeVFX;
    private FarmersFavour passive;

    public class FarmersFavour : Passive
    {
        public FarmersFavour(float[] durs, GameObject owner) : base(durs) { Owner = owner; }

        public bool wIsActive;
        private float healedDuringW = 0f;
        private GameObject Owner;

        public List<GameObject> AllFavours = new List<GameObject>();

        public override void Initialize()
        {
            HealthController.OnDamageTaken += GiveFarmersFavour;
        }

        public void GiveFarmersFavour(DamageData dd)
        {
            if (!wIsActive)
                return;

            Debug.Log("giving w passive");

            dd.Dealer.GetComponent<StatusController>()?.AddStatusEffect(Effect);
            AllFavours.Add(dd.Dealer);
        }

        public void AddPassiveHeal(float v)
        {
            if (!wIsActive)
                return;

            healedDuringW += v;
        }

        public void PopFarmersFavour(Entity receiver)
        {
            DamageData dmgData = new DamageData(
                Owner,
                receiver.gameObject,
                healedDuringW,
                EFFECTOR_TYPE.Flat,
                DAMAGE_TYPE.Magical);

            receiver.GetComponent<HealthController>().TakeDamage(dmgData);
            Effect.OnEffectEnd -= PopFarmersFavour;
        }

        public void Reset()
        {
            healedDuringW = 0f;
            wIsActive = false;
            AllFavours.Clear();
        }
    }

    public override void Fire(Champion caster)
    {
        if (champ.Casting)
            return;

        passive.AllFavours.Clear();

        SubMethods();

        YuutsuController.OnPassiveTick += passive.AddPassiveHeal;
        passive.Effect.OnEffectEnd += passive.PopFarmersFavour;

        caster.GetComponent<StatusController>().AddStatusEffect(yuutsuWActive);
        champ.Casting = true;
    }

    private void OnAbilityTrigger(Entity receiver)
    {
        DamageData dd = (Data as DamageAbilityData).Values[champ.GetAbilityLevel(1) - 1];
        champ.AddResistances(dd.DamageType, dd.Value);

        timer = 0f;
        passive.wIsActive = true;

        champ.StopMoving();
        activeVFX = champ.GetComponent<VFXController>().ActivateVFX(Data.VFX[0]);
    }
    private void OnAbilityUpdate(Entity receiver, float deltaTime)
    {
        timer += deltaTime;

        if(timer >= Durations[champ.GetAbilityLevel(1) - 1])
        {
            receiver.GetComponent<StatusController>().RemoveStatusEffect(yuutsuWActive);
            activeVFX = receiver.GetComponent<VFXController>().DestroyVFX(activeVFX);
            timer = 0;
        }
    }
    private void OnAbilityComplete(Entity receiver)
    {
        for (int i = 0; i < passive.AllFavours.Count; i++)
        {
            passive.AllFavours[i].GetComponent<StatusController>().RemoveStatusEffect(passive.Effect);
        }

        champ.CancelPath();
        champ.RemoveResistances(DAMAGE_TYPE.Mixed, (Data as DamageAbilityData).Values[champ.GetAbilityLevel(1) - 1].Value);

        UnsubMethods();

        passive.Reset();
        champ.Casting = false;
    }

    private void UnsubMethods()
    {
        yuutsuWActive.OnEffectStart -= OnAbilityTrigger;
        yuutsuWActive.OnUpdate      -= OnAbilityUpdate;
        yuutsuWActive.OnEffectEnd   -= OnAbilityComplete;
    }
    private void SubMethods()
    {
        yuutsuWActive.OnEffectStart += OnAbilityTrigger;
        yuutsuWActive.OnUpdate      += OnAbilityUpdate;
        yuutsuWActive.OnEffectEnd   += OnAbilityComplete;
    }
}

