using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MOBA.AI
{
    public class StateMachine<T>
    {
        public State<T> CurrentState { get; private set; }
        public T Owner;

        public StateMachine(T _o)
        {
            Owner = _o;
            CurrentState = null;
        }

        public void ChangeState(State<T> newState)
        {
            CurrentState?.ExitState(Owner);
            CurrentState = newState;
            CurrentState.EnterState(Owner);
        }

        public void UpdateState()
        {
            CurrentState?.UpdateState(Owner);
        }
    }

    public abstract class State<T>
    {
        public abstract void EnterState(T owner);
        public abstract void UpdateState(T owner);
        public abstract void ExitState(T owner);
    }
}
