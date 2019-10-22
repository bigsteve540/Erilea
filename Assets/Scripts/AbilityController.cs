using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityController : MonoBehaviour
{
    private Champion me;

    public float CastTimeCurrent { get; private set; }
    public float TotalCastTime { get; private set; }
    private Ability toCast;
    public bool abilityActive;

    void Start() { me = GetComponent<Champion>(); }

    public void Cast(ABILITY_CODE key)
    {
        if (me.Casting || abilityActive)
            return;

        int index = Convert.ToInt32(key);
        toCast = me.GetAbility(index);

        if (me.GetAbilityData(index).CastTime > 0f)
        {
            me.StopMoving();
            TotalCastTime = me.GetAbilityData(index).CastTime;
            abilityActive = true;
        }
        else
        {
            toCast.Fire(gameObject);
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
                toCast.Fire(gameObject);
                me.UnlockMovement();

                toCast = null;
                CastTimeCurrent = 0f;
                me.Casting = false;
            }
        }
    }
}
