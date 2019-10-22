﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [HideInInspector]
    public Transform VFXPoint { get; private set; }

    private List<GameObject> ActiveVisuals = new List<GameObject>();

    void OnEnable() { VFXPoint = transform.GetChild(1).GetChild(1); }

    public GameObject ActivateVFX(GameObject effect)
    {
        GameObject instance = Instantiate(effect, VFXPoint);
        ActiveVisuals.Add(instance);
        return instance;
    }
    public void DestroyVFX(GameObject effect)
    {
        bool removed = ActiveVisuals.Remove(effect);

        if (removed)
            Destroy(effect);
        else
            throw new System.Exception("VFX Not found. Aborting destruction.");
    }

    void Update()
    {
        
    }
}
