using UnityEngine;

public class Root : CCEffect
{
    public override void OnCCStart(Entity receiver)
    {
        receiver.StopMoving();
    }

    public override void OnCCUpdate(Entity receiver, float deltaTime)
    {
        currentTime += deltaTime;

        if (currentTime >= Duration)
        {
            receiver.GetComponent<CCController>().RemoveCC(this);
            currentTime = 0f;
        }
    }

    public override void OnCCEnd(Entity receiver)
    {
        receiver.CancelPath();
        Terminate();
    }

    public Root(GameObject dealer, float dur) : base(dealer, dur) { } 
}
