using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Champion champion;

    void Start()
    {
        champion = FindObjectOfType<Champion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            champion.Cast(ABILITY_CODE.Q);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            champion.Cast(ABILITY_CODE.W);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            champion.Cast(ABILITY_CODE.E);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            champion.Cast(ABILITY_CODE.R);
        }

        if (Input.GetKeyDown(KeyCode.S)) { champion.StopMoving(); }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {

                if (hit.transform.CompareTag("Ground"))
                {
                    champion.Move(hit.point);
                }

                if (hit.transform.CompareTag("Entity") || hit.transform.CompareTag("Champion") && Vector3.Distance(hit.transform.position, champion.transform.position) > champion.AttackRange)
                {
                    //champion.MoveToAttack(hit.transform.gameObject);
                }
                
            }
        }

        

    }
}
