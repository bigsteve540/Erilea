using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuutsuQ : Ability
{
    public YuutsuQ(Champion c) : base(c, "Champions/Yuutsu/Abilities/Yuutsu Q") { }

    public override void Fire(Champion caster)
    {
        if (champ.Casting)
            return;

        //Trigger Visual
        for (int i = 0; i < Data.TargetTags.Length; i++)
        {
            Collider[] outerHits = FetchTargets.ByCircle(caster.gameObject, Data.CastDistance, Data.TargetTags[i]);
            Collider[] innerHits = FetchTargets.ByCircle(caster.gameObject, Data.CastDistance / 2, Data.TargetTags[i]);

            CheckRadii(caster, outerHits, innerHits);
        }
    }

    public void CheckRadii(Champion caster, Collider[] a, Collider[] b)
    {
        DamageData[] dmgVals = (Data as DamageAbilityData).Values;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i].transform.parent?.gameObject == caster.gameObject)
                continue;

            bool found = false;

            for (int j = 0; j < b.Length; j++)
            {
                if (a[i] == b[j])
                {
                    //inside both, regular damage.
                    found = true;
                    b[j].transform.parent?.GetComponent<HealthController>()
                        .TakeDamage(dmgVals[champ.GetAbilityLevel(0) -1]);
                }
            }

            if (!found)
            {
                //collider only in outer, deal 150% damage.
                DamageData multiplied = new DamageData(
                    caster.gameObject,
                    a[i].gameObject,
                    dmgVals[champ.GetAbilityLevel(0) - 1].Value * 1.5f,
                    dmgVals[champ.GetAbilityLevel(0) - 1].Type ,
                    dmgVals[champ.GetAbilityLevel(0) - 1].DamageType
                    );

                a[i].transform.parent?.GetComponent<HealthController>().TakeDamage(multiplied);
            }
        }
    }
}
