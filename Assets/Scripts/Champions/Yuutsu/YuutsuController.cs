using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuutsuController : Champion
{
    public override string Name => "Yuutsu";

    private float DamageTaken = 0f;
    private float PassiveHealTick = 0f;

    private IEnumerator ActivePassive = null;

    protected override void Awake()
    {
        base.Awake();
        OnDamageTaken += TriggerPassive;
    }

    public override void ModifyHealth(ValueEffector v)
    {
        if (v.IsNegative)
        {
            v.Value *= 1.2f;
            base.ModifyHealth(v);
        }
        else
        {
            base.ModifyHealth(v);
        }   
    }

    public void TriggerPassive(float v)
    {
        DamageTaken += v * 0.5f;
        PassiveHealTick = DamageTaken * 0.1f;

        if (ActivePassive == null)
        {
            ActivePassive = CheckPassiveConditions();
            StartCoroutine(CheckPassiveConditions());
        }
        Debug.Log("Passive Triggered: " + ActivePassive);
    }

    protected override IEnumerator CheckPassiveConditions()
    {
        GameObject visual = Instantiate(ChampionStats.VFX[0], VFXPoint);

        while (DamageTaken > totalHealed) //damageTaken gets recalculated upon taking new damage, fix loop to accomodate this.
        {
            yield return new WaitForSecondsRealtime(1f);
            Health += PassiveHealTick;
            DamageTaken += PassiveHealTick;
            Debug.Log("Tick");
        }
        DamageTaken = 0;
        ActivePassive = null;
        Destroy(visual);
        yield return null;
    }
}
