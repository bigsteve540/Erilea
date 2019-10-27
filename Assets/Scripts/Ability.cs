using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    protected Champion champ;
    public AbilityData Data { get; private set; }

    public abstract void Fire(Champion caster);

    public Ability(Champion c, string dataFilePath)
    {
        champ = c;
        Data = Resources.Load(dataFilePath) as AbilityData;
    }
}


