using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockup : CCEffect
{

    public Vector3 NewPosition;

    public override void OnCCStart(Entity receiver)
    {
        receiver.StopMoving();
    }

    public override void OnCCEnd(Entity receiver)
    {
        receiver.StopMoving();
    }

    public override void OnCCUpdate(Entity receiver, float deltaTime)
    {
        Terminate();
    }

    private float GetPeakFlightHeight()
    {
        return 5f;
    }
    private Vector3 GetEndPosition(GameObject receiver, Vector3 unitsMoved)
    {
        return new Vector3(
            receiver.gameObject.transform.position.x + unitsMoved.x,
            receiver.gameObject.transform.position.y + unitsMoved.y, //work out smth for recalculating y based on ground height
            receiver.gameObject.transform.position.z + unitsMoved.z
            );
    }

    public Knockup(GameObject dealer, Vector3 newPosition, float dur) : base(dealer, dur)
    {
        NewPosition = GetEndPosition(dealer, newPosition);
    }
}
