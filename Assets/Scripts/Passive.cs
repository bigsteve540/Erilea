using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive
{
    public float[] Durations;
    public StatusEffect Effect { get; private set; }

    public abstract void Initialize();

    public Passive(float[] durs)
    {
        Durations = durs;
        Effect = new StatusEffect();
        Initialize();
    }
}
