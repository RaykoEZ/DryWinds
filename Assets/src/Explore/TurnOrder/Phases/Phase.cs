using System;
using UnityEngine;
namespace Curry.Explore
{
    // Each turn will contain different phases
    [Serializable]
    public abstract class Phase
    {
        public event OnTurnPhaseTransition OnGameStateTransition;
        protected Type NextState = default;

        public abstract void Init();

        public virtual void OnEnter(Phase incomingState) 
        {
            Evaluate();
            TransitionTo();
        }
        // Type returned = the next game state.
        protected abstract void Evaluate();
        protected virtual Type TransitionTo()
        {
            OnGameStateTransition?.Invoke(NextState);
            return NextState;
        }

    }

}