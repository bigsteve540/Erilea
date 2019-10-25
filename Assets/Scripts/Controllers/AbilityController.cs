using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityController : Controller
{
    public float CastTimeCurrent { get; private set; }
    public float TotalCastTime { get; private set; }
    private Ability toCast;
    public bool abilityActive;
    private Champion champion;

    protected override void Start() { base.Start();  champion = target as Champion; }

    public void Cast(ABILITY_CODE key)
    {
        if (champion.Casting || abilityActive)
            return;

        int index = Convert.ToInt32(key);
        toCast = champion.GetAbility(index);

        if (champion.GetAbilityData(index).CastTime > 0f)
        {
            champion.StopMoving();
            TotalCastTime = champion.GetAbilityData(index).CastTime;
            abilityActive = true;
        }
        else
        {
            toCast.Fire(champion);
        }
        //trigger vfx or smth
    }

    void Update()
    {
        if (abilityActive)
        {
            CastTimeCurrent += Time.deltaTime;

            if(CastTimeCurrent >= TotalCastTime)
            {
                toCast.Fire(champion);
                champion.UnlockMovement();

                toCast = null;
                CastTimeCurrent = 0f;
                champion.Casting = false;
            }
        }
    }
}
