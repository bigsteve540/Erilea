using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Firing");
        other.transform.parent.GetComponent<Champion>().ModifyHealth(new ValueEffector(true, 100f, EFFECTOR_TYPE.Flat));
    }
}
