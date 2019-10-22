using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MovementController : Controller
{
    private List<SpeedEffector> MSMods = new List<SpeedEffector>();

    public void AddMoveSpeedMod(SpeedEffector se)
    {
        MSMods.Add(se);
        MSMods = MSMods.OrderBy(o => Convert.ToInt32(o.Type)).ToList();
        target.MoveSpeed = CalculateSpeed();
    }
    public void RemoveMoveSpeedMod(SpeedEffector se)
    {
        MSMods.Remove(se);
        target.MoveSpeed = CalculateSpeed();
        MSMods = MSMods.OrderBy(o => Convert.ToInt32(o.Type)).ToList();
    }

    public float CalculateSpeed()
    {
        if (MSMods.Count < 1)
            return target.GetBaseMS(); //do more rigorous checking for vfx

        float sumFlats = 0f;
        float sumAdditives = 0f; ;
        float highestSlowPercent = 0f;

        List<int> indexOfMultiplicatives = new List<int>();

        for (int i = 0; i < MSMods.Count; i++)
        {
            switch (MSMods[i].Type)
            {
                case VALUE_TYPE.Flat:

                    if (MSMods[i].IsNegative)
                    { sumFlats -= MSMods[i].Value; }
                    else
                    { sumFlats += MSMods[i].Value; }
                    break;

                case VALUE_TYPE.Additive:

                    if (MSMods[i].IsNegative)
                    { sumAdditives -= MSMods[i].Value; }
                    else
                    { sumAdditives += MSMods[i].Value; }
                    break;

                case VALUE_TYPE.Multiplicative:
                    indexOfMultiplicatives.Add(i);
                    break;
            }

            if (MSMods[i].IsNegative && MSMods[i].Type == VALUE_TYPE.Multiplicative)
            {
                if (MSMods[i].Value > highestSlowPercent)
                    highestSlowPercent = MSMods[i].Value;
            }
        }

        float processedSpeed = target.MoveSpeed;
        processedSpeed += sumFlats * (1 + sumAdditives) * (1 - highestSlowPercent);

        for (int i = 0; i < indexOfMultiplicatives.Count; i++)
        {
            processedSpeed *= MSMods[indexOfMultiplicatives[i]].Value;
        }

        //do softcaps

        return processedSpeed;
    }
}
