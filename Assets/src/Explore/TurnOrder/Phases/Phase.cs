using System;
using UnityEngine;
namespace Curry.Explore
{
    public delegate void OnPhaseInterrupt(Phase sender);
    // Each turn will contain different phases
    [Serializable]
    public abstract class Phase
    {
        public event OnTurnPhaseTransition OnGameStateTransition;
        // Trigger interrupt for UI and others
        public event OnPhaseInterrupt OnInterrupt;
        protected Type NextState = default;

        public abstract void Init();

        public virtual void OnEnter(Phase incomingState) 
        {
            Evaluate();
            TransitionTo();
        }
        public virtual void Pause() { }
        public virtual void Resume() { }
        protected void Interrupt()
        {
            OnInterrupt?.Invoke(this);
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