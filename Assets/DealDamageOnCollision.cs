using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnCollision : MonoBehaviour
{

    public void Update()
    {
        float step = 2f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, FindObjectOfType<Champion>().transform.position, step);
    }

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
        other.transform.parent.GetComponent<CCController>().ApplyCC(new Slow(gameObject, 100f, false, 2f));
    }
}
