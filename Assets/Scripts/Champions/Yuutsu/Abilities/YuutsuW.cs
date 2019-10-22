using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuutsuW : Ability
{
    private StatusEffect YuutsuWActive;
    private DamageAbilityData data;

    public YuutsuW(Champion c) : base(c)
    {
        data = Resources.Load("Champions/Yuutsu/Abilities/Yuutsu W") as DamageAbilityData;
        YuutsuWActive = new StatusEffect
        {
            Durations = new float[5] { 2f, 2.5f, 3f, 3.5f, 4f },
            StatusEffectTriggers = new StatusEffectData()
        };
    }
    public override AbilityData GetData()
    {
        return data;
    }

    public override void Fire(Champion caster)
    {
        if (champ.Casting)
            return;

        YuutsuWActive.StatusEffectTriggers.OnEffectStart += OnAbilityTrigger;
        YuutsuWActive.StatusEffectTriggers.OnUpdate += OnAbilityUpdate;
        YuutsuWActive.StatusEffectTriggers.OnEffectEnd += OnAbilityComplete;

        caster.GetComponent<StatusController>().AddStatusEffect(YuutsuWActive);
        champ.Casting = true;
    }

    private GameObject activeVFX;
    public override void OnAbilityTrigger(Entity caster)
    {
        activeVFX = caster.GetComponent<VFXController>().ActivateVFX(data.VFX[0]);

        champ.StopMoving();

        DamageData dd = data.Values[champ.GetAbilityLevel(1) - 1];
        champ.AddResistances(dd.DamageType, dd.Value);

        timer = 0f;
    }

    private float timer;
    public override void OnAbilityUpdate(Entity caster, float deltaTime)
    {
        timer += deltaTime;

        //TODO: check for people attempting to hit yuutsu :: give them farmer's favour

        if(timer >= YuutsuWActive.Durations[champ.GetAbilityLevel(1) - 1])
        {
            caster.GetComponent<StatusController>().RemoveStatusEffect(YuutsuWActive);
            caster.GetComponent<VFXController>().DestroyVFX(activeVFX);
            activeVFX = null;
            timer = 0;
        }
    }

    public override void OnAbilityComplete(Entity caster)
    {
        champ.CancelPath();
        champ.RemoveResistances(DAMAGE_TYPE.Mixed, data.Values[champ.GetAbilityLevel(1) - 1].Value);
        YuutsuWActive.StatusEffectTriggers.OnEffectStart -= OnAbilityTrigger;
        YuutsuWActive.StatusEffectTriggers.OnUpdate -= OnAbilityUpdate;
        YuutsuWActive.StatusEffectTriggers.OnEffectEnd -= OnAbilityComplete;
        champ.Casting = false;
    }
}

