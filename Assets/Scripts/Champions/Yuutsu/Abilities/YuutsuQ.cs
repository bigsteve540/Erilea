using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Yuutsu Q", menuName = "Champions/Yuutsu/Q")]
public class YuutsuQ : Ability
{
    public override void Fire(GameObject caster, int abilityLevelNonIndexed)
    {
        for (int i = 0; i < TargetTags.Length; i++)
        {
            Collider[] outerHits = FetchTargets.ByCircle(caster, CastDistance, TargetTags[i]);
            Collider[] innerHits = FetchTargets.ByCircle(caster, CastDistance / 2, TargetTags[i]);

            CheckRadii(caster, abilityLevelNonIndexed, outerHits, innerHits);
        }
    }

    public void CheckRadii(GameObject caster, int abilityLevelNonIndexed, Collider[] a, Collider[] b)
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i].transform.gameObject == caster)
                continue;

            bool found = false;

            for (int j = 0; j < b.Length; j++)
            {
                if (a[i] == b[j])
                {
                    //inside both, regular damage.
                    found = true;
                    b[j].transform.parent?.GetComponent<Champion>().ModifyHealth(AbilityValues[abilityLevelNonIndexed - 1]);
                }
            }

            if (!found)
            {
                //collider only in outer, deal 150% damage.
                ValueEffector multiplied = new ValueEffector(
                    AbilityValues[abilityLevelNonIndexed - 1].IsNegative,
                    AbilityValues[abilityLevelNonIndexed - 1].Value * 1.5f,
                    AbilityValues[abilityLevelNonIndexed - 1].Type
                    );

                a[i].transform.parent?.GetComponent<Champion>().ModifyHealth(multiplied);
            }
        }
    }
}
