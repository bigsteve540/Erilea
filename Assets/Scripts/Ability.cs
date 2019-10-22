using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public abstract AbilityData GetData();
    protected Champion champ;

    public abstract void Fire(Champion caster);

    public virtual void OnAbilityTrigger(Entity caster) { }
    public virtual void OnAbilityComplete(Entity caster) { }
    public virtual void OnAbilityUpdate(Entity caster, float deltaTime) { }

    public Ability(Champion c)
    {
        champ = c;
    }
}


