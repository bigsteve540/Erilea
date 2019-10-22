using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    protected List<StatusEffect> ActiveStatusEffects = new List<StatusEffect>();

    public void AddStatusEffect(StatusEffect effect)
    {
        ActiveStatusEffects.Add(effect);
        effect.StatusEffectTriggers?.OnEffectStart?.Invoke(gameObject);
    }
    public void RemoveStatusEffect(StatusEffect effect)
    {
        ActiveStatusEffects.Remove(effect);
        effect.StatusEffectTriggers?.OnEffectEnd?.Invoke(gameObject);
    }

    void Update()
    {
        for (int i = 0; i < ActiveStatusEffects.Count; i++)
        {
            ActiveStatusEffects[i].StatusEffectTriggers?.OnUpdate?.Invoke(gameObject, Time.deltaTime);
        }
    }
}
