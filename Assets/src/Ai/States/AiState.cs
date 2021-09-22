using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public delegate void OnAiStateTransition(AiState next);
    [Serializable]
    public abstract class AiState : ScriptableObject, IAiAction
    {
        [SerializeField] protected float m_basePriority = default;
        public event OnAiStateTransition OnTransition;
        public event OnActionFinish OnFinish;
        public bool ActionInProgress { get; private set; }

        public abstract bool PreCondition(NpcWorldState args);
        public abstract bool ExitCondition(NpcWorldState args);

        public abstract void OnEnter(NpcController controller, NpcWorldState state);
        public abstract void OnUpdate(NpcController controller, NpcWorldState state);
        public virtual void TransitionTo(AiState next) 
        {
            OnTransition?.Invoke(next);
        }
    }
}
