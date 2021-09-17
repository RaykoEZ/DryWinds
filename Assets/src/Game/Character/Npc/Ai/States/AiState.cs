using System;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnAiStateTransition(AiState next);
    [Serializable]
    public abstract class AiState : ScriptableObject
    { 
        public event OnAiStateTransition OnTransition;
        public abstract void OnEnter(NpcController controller);
        public abstract void OnUpdate(NpcController controller);
        public virtual void TransitionTo(AiState next) 
        {
            OnTransition?.Invoke(next);
        }
    }
}
