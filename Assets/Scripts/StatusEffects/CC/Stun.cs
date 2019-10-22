using UnityEngine;

public class Stun : CCEffect
{
    public override void OnCCStart(Entity receiver)
    {
        //find vfx controller and fire stun fx
        receiver.StopMoving();

        if (receiver as Champion != null)
            (receiver as Champion).Casting = true;
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
        receiver.CancelPath();

        if (receiver as Champion != null)
            (receiver as Champion).Casting = false;

        Terminate();
    }
    
    public Stun(GameObject dealer, float dur) : base(dealer, dur) { }
}
