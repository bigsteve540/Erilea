using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public abstract AbilityData GetData();
    protected Champion champ;

    public abstract void Fire(GameObject caster);

    public virtual void OnAbilityTrigger(GameObject caster) { }
    public virtual void OnAbilityComplete(GameObject caster) { }
    public virtual void OnAbilityUpdate(GameObject caster, float deltaTime) { }

    public Ability(Champion c)
    {
        champ = c;
    }
}


