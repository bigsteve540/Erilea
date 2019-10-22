using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuutsuController : Champion
{
    public override string Name => "Yuutsu";

    protected override string Filepath => "Champions/Yuutsu/Yuutsu";

    private float DamageTaken = 0f;
    private float PassiveHealTick = 0f;

    protected override void LoadAbilities()
    {
        abilities = new Ability[2] 
        {
            new YuutsuQ(this),
            new YuutsuW(this)
            //add remaining later
        };
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        HealthController.OnDamageTaken += TriggerPassive;
    }

    public void TriggerPassive(DamageData dd)
    {
        if (dd.Receiver != gameObject)
            return;

        DamageTaken += dd.Value * 0.5f;
        PassiveHealTick = DamageTaken * 0.1f;

        StopCoroutine(CheckPassiveConditions());

        if (ActivePassive == null)
        {
            ActivePassive = CheckPassiveConditions();
            StartCoroutine(CheckPassiveConditions());
        }
    }

    //change to update loop?

    private IEnumerator ActivePassive = null;

    protected override IEnumerator CheckPassiveConditions()
    {
        GameObject activeVFX = GetComponent<VFXController>().ActivateVFX(stats.VFX[0]);

        while (DamageTaken >= PassiveHealTick)
        {
            yield return new WaitForSecondsRealtime(1f);
            Health += PassiveHealTick;
            DamageTaken -= PassiveHealTick;
        }
        DamageTaken = 0;
        ActivePassive = null;
        GetComponent<VFXController>().DestroyVFX(activeVFX);
        yield return null;
    }
}
