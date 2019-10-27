using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : Controller
{
    protected List<StatusEffect> ActiveStatusEffects = new List<StatusEffect>();

    public void AddStatusEffect(StatusEffect effect)
    {
        ActiveStatusEffects.Add(effect);
        effect.OnEffectStart?.Invoke(target);
    }
    public void RemoveStatusEffect(StatusEffect effect)
    {
        ActiveStatusEffects.Remove(effect);
        effect.OnEffectEnd?.Invoke(target);
    }

    void Update()
    {
        for (int i = 0; i < ActiveStatusEffects.Count; i++)
        {
            ActiveStatusEffects[i].OnUpdate?.Invoke(target, Time.deltaTime);
        }
    }
}
