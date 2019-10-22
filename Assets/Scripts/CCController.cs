using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCController : MonoBehaviour
{
    private List<CCEffect> ActiveCC = new List<CCEffect>();
    private Entity target;

    private void OnEnable()
    {
        target = GetComponent<Entity>();
    }

    public void ApplyCC(CCEffect cc)
    {
        ActiveCC.Add(cc);
        cc.StatusEffectTriggers?.OnEffectStart?.Invoke(target);
    }
    public void RemoveCC(CCEffect cc)
    {
        ActiveCC.Remove(cc);
        cc.StatusEffectTriggers?.OnEffectEnd?.Invoke(target);
    }
    public void PurgeCC(CCEffect[] types = null)
    {
        if(types == null)
        {
            for (int i = 0; i < ActiveCC.Count; i++)
            {
                ActiveCC[i].StatusEffectTriggers?.OnEffectEnd?.Invoke(GetComponent<Entity>());
            }
            ActiveCC.Clear();
        }
        else
        {
            for (int i = 0; i < types.Length; i++)
            {
                for (int j = ActiveCC.Count; j > 0; j--)
                {
                    if(ActiveCC[j].GetType() == types[i].GetType())
                    {
                        ActiveCC.RemoveAt(j);
                        types[i].StatusEffectTriggers?.OnEffectEnd?.Invoke(GetComponent<Entity>());
                    }
                }
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < ActiveCC.Count; i++)
        {
            ActiveCC[i].StatusEffectTriggers?.OnUpdate?.Invoke(target, Time.deltaTime);
        }
    }
}
