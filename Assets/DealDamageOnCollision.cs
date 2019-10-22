﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent.GetComponent<HealthController>().TakeDamage(
            new DamageData(
                gameObject,
                other.gameObject,
                100f,
                EFFECTOR_TYPE.Flat,
                DAMAGE_TYPE.True
                )
        );
    }
}
