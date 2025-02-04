﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CCEffect : StatusEffect
{
    public float Duration { get; protected set; }
    public GameObject Dealer { get; protected set; }
    protected float currentTime;

    private void Initialize()
    {
        OnEffectStart += OnCCStart;
        OnUpdate += OnCCUpdate;
        OnEffectEnd += OnCCEnd;
    }
    protected void Terminate()
    {
        OnEffectStart -= OnCCStart;
        OnUpdate -= OnCCUpdate;
        OnEffectEnd -= OnCCEnd;
    }

    public abstract void OnCCStart(Entity receiver);
    public abstract void OnCCUpdate(Entity receiver, float deltaTime);
    public abstract void OnCCEnd(Entity receiver);

    public CCEffect(GameObject dealer, float dur)
    {
        Dealer = dealer;
        Duration = dur;
        Initialize();
    }
}

