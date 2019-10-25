using MOBA.AI;

public class IdleState : State<MinionAI>
{
    private static IdleState _instance;
    private IdleState()
    {
        if(_instance != null)
            return;
        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if(_instance == null)
            {
                new IdleState();
            }
            return _instance;
        }
    }


    public override void EnterState(MinionAI owner)
    {
      
    }

    public override void ExitState(MinionAI owner)
    {
    
    }

    public override void UpdateState(MinionAI owner)
    {
        
    }
}


