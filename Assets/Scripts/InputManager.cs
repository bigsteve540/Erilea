﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    AbilityController ac;
    [SerializeField]
    Champion champion = null;

    private void Start()
    {
        ac = champion.GetComponent<AbilityController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ac.Cast(ABILITY_CODE.Q);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ac.Cast(ABILITY_CODE.W);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ac.Cast(ABILITY_CODE.E);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ac.Cast(ABILITY_CODE.R);
        }

        if (Input.GetKeyDown(KeyCode.S)) { champion.CancelPath(); }

        if (Input.GetKeyDown(KeyCode.L)) { champion.LevelUp(); }

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
