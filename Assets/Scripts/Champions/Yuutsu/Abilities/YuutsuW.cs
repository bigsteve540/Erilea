using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuutsuW : Ability
{
    private StatusEffect yuutsuWActive;
    private GameObject activeVFX;
    private bool wIsActive;
    private float timer;
    private float healedDuringW = 0f;
    private DamageAbilityData data;
    private StatusEffect farmersFavour;
    private List<GameObject> allFavours = new List<GameObject>(); 

    public YuutsuW(Champion c) : base(c)
    {
        data = Resources.Load("Champions/Yuutsu/Abilities/Yuutsu W") as DamageAbilityData;
        yuutsuWActive = new StatusEffect
        {
            Durations = new float[5] { 2f, 2.5f, 3f, 3.5f, 4f },
            StatusEffectTriggers = new StatusEffectData()
        };
        HealthController.OnDamageTaken += GiveFarmersFavour;
        farmersFavour = new StatusEffect
        {
            Durations = new float[1] { 6f },
            StatusEffectTriggers = new StatusEffectData()
        };
    }
    public override AbilityData GetData()
    {
        return data;
    }

    public void GiveFarmersFavour(DamageData dd)
    {
        if (!wIsActive)
            return;

        dd.Dealer.GetComponent<StatusController>()?.AddStatusEffect(farmersFavour);
        allFavours.Add(dd.Dealer);
    }
    public void PopFarmersFavour(Entity receiver)
    {
        DamageData Wpassive = new DamageData(
            champ.gameObject, 
            receiver.gameObject, 
            healedDuringW, 
            EFFECTOR_TYPE.Flat, 
            DAMAGE_TYPE.Magical);

        receiver.GetComponent<HealthController>().TakeDamage(Wpassive);
        farmersFavour.StatusEffectTriggers.OnEffectEnd -= PopFarmersFavour;
    }
    public void AddPassiveHeal(float v)
    {
        if (!wIsActive)
            return;

        healedDuringW += v;
    }

    public override void Fire(Champion caster)
    {
        if (champ.Casting)
            return;

        allFavours.Clear();

        yuutsuWActive.StatusEffectTriggers.OnEffectStart += OnAbilityTrigger;
        yuutsuWActive.StatusEffectTriggers.OnUpdate += OnAbilityUpdate;
        yuutsuWActive.StatusEffectTriggers.OnEffectEnd += OnAbilityComplete;

        YuutsuController.OnPassiveTick += AddPassiveHeal;
        farmersFavour.StatusEffectTriggers.OnEffectEnd += PopFarmersFavour;

        caster.GetComponent<StatusController>().AddStatusEffect(yuutsuWActive);
        champ.Casting = true;
    }
    public override void OnAbilityTrigger(Entity receiver)
    {
        activeVFX = receiver.GetComponent<VFXController>().ActivateVFX(data.VFX[0]);

        champ.StopMoving();

        DamageData dd = data.Values[champ.GetAbilityLevel(1) - 1];
        champ.AddResistances(dd.DamageType, dd.Value);

        timer = 0f;
        wIsActive = true;
    }
    public override void OnAbilityUpdate(Entity receiver, float deltaTime)
    {
        timer += deltaTime;

        if(timer >= yuutsuWActive.Durations[champ.GetAbilityLevel(1) - 1])
        {
            receiver.GetComponent<StatusController>().RemoveStatusEffect(yuutsuWActive);
            receiver.GetComponent<VFXController>().DestroyVFX(activeVFX);
            activeVFX = null;
            timer = 0;
        }
    }
    public override void OnAbilityComplete(Entity receiver)
    {
        for (int i = 0; i < allFavours.Count; i++)
        {
            allFavours[i].GetComponent<StatusController>().RemoveStatusEffect(farmersFavour);
        }

        champ.CancelPath();
        champ.RemoveResistances(DAMAGE_TYPE.Mixed, data.Values[champ.GetAbilityLevel(1) - 1].Value);
        yuutsuWActive.StatusEffectTriggers.OnEffectStart -= OnAbilityTrigger;
        yuutsuWActive.StatusEffectTriggers.OnUpdate -= OnAbilityUpdate;
        yuutsuWActive.StatusEffectTriggers.OnEffectEnd -= OnAbilityComplete;

        healedDuringW = 0f;

        champ.Casting = false;
        wIsActive = false;
    }
}

