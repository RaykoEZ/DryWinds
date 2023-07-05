using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public delegate void OnPhaseInterrupt(Phase sender);
    // Each turn will contain different phases
    [Serializable]
    public abstract class Phase : MonoBehaviour
    {
        [SerializeField] string m_displayName = default;
        public event OnTurnPhaseTransition OnGameStateTransition;
        protected abstract Type NextState { get; set; }
        public string Name => m_displayName;
        public virtual void Init() { }
        public virtual void OnEnter(Phase incomingState) 
        {
            Evaluate();
        }
        public virtual void Pause() { }
        // Type returned = the next game state.
        protected void Evaluate() 
        {
            StartCoroutine(Evaluate_Internal());
        }
        protected abstract IEnumerator Evaluate_Internal();
        protected virtual void TransitionTo()
        {
            OnGameStateTransition?.Invoke(NextState);
        }
    }
}