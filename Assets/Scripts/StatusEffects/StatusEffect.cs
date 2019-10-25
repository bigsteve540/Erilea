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
    public delegate void EventStateHandler(Entity receiver);
    public delegate void UpdateEventHandler(Entity receiver, float deltaTime);

    public EventStateHandler OnEffectStart;
    public EventStateHandler OnEffectEnd;
    public UpdateEventHandler OnUpdate;
}
