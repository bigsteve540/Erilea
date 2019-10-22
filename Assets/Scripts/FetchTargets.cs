using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FetchTargets
{
    public static Collider ByTargeted(GameObject caster, float castDist, string targetTag)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag(targetTag))
            {
                if (Vector3.Distance(hit.transform.position, caster.transform.position) <= castDist)
                {
                    return hit.collider;
                }
            }
        }
        return null;
    }

    public static Collider[] ByLinear(GameObject caster, float width, float length, string targetTag)
    {
        Collider[] hits = Physics.OverlapBox(new Vector3(caster.transform.position.x, caster.transform.position.y, caster.transform.position.z - width / 2f), new Vector3((width / 2f), 1.05f, (length / 2f)));
        List<Collider> validHits = new List<Collider>();

        validHits.Clear();

        for (int i = 0; i < hits.Length - 1; i++)
        {
            if (hits[i].transform.CompareTag(targetTag))
            {
                validHits.Add(hits[i]);
            }
        }
        return validHits.ToArray();
    }

    public static Collider[] ByCircle(GameObject caster, float radius, string targetTag)
    {
        Collider[] hits = Physics.OverlapSphere(caster.transform.position, radius);
        List<Collider> validHits = new List<Collider>();

        validHits.Clear();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag(targetTag))
            {
                validHits.Add(hits[i]);
            }
        }
        return validHits.ToArray();
    }
    public static Collider[] ByCircle(GameObject caster, Vector3 origin, Vector3 dir, float radius, string targetTag, float arcAngle)
    {
        Collider[] hits = Physics.OverlapSphere(origin, radius);
        List<Collider> validHits = new List<Collider>();

        validHits.Clear();

        for (int i = 0; i < hits.Length; i++)
        {
            Vector3 v2Collider = (hits[i].transform.position - origin).normalized;

            if (Vector3.Dot(v2Collider, dir) > (1 - (arcAngle / 360) * 2))
            {
                if (hits[i].transform.CompareTag(targetTag))
                {
                    validHits.Add(hits[i]);
                }
            }
        }
        return validHits.ToArray();
    }
}
