using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : CCEffect
{
    public SpeedEffector slowData { get; private set; }

    public override void OnCCStart(Entity receiver)
    {
        receiver.GetComponent<MovementController>().AddMoveSpeedMod(slowData);
    }

    public override void OnCCUpdate(Entity receiver, float deltaTime)
    {
        currentTime += deltaTime;

        if(currentTime >= Duration)
        {
            receiver.GetComponent<CCController>().RemoveCC(this);
            currentTime = 0f;
        }
    }

    public override void OnCCEnd(Entity receiver)
    {
        receiver.GetComponent<MovementController>().RemoveMoveSpeedMod(slowData);
        Terminate();
    }

    public Slow(GameObject dealer, float val, bool isNeg, VALUE_TYPE type, float dur) : base(dealer, dur)
    {
        slowData = new SpeedEffector(val, isNeg, type);
    }
    public Slow(GameObject dealer, float val, float dur) : this(dealer, val, true, VALUE_TYPE.Flat, dur) { }
    public Slow(GameObject dealer, float val, bool isNeg, float dur) : this(dealer, val, isNeg, VALUE_TYPE.Flat, dur) { }
}

public enum VALUE_TYPE { Flat = 100, Additive = 200, Multiplicative = 300 }
public struct SpeedEffector
{
    public float Value;
    public bool IsNegative;
    public VALUE_TYPE Type;

    public SpeedEffector(float v, bool isNegative = true, VALUE_TYPE t = VALUE_TYPE.Flat)
    {
        Value = v;
        Type = t;
        IsNegative = isNegative;
    }
}
