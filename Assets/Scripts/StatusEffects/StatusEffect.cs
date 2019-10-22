using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect
{
    public float[] Durations;
    public StatusEffectData StatusEffectTriggers;
}

public class StatusEffectData
{
    public delegate void EventStateHandler(Entity caster);
    public delegate void UpdateEventHandler(Entity caster, float deltaTime);

    public EventStateHandler OnEffectStart;
    public EventStateHandler OnEffectEnd;
    public UpdateEventHandler OnUpdate;
}
