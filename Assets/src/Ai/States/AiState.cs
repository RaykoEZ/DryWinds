using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public delegate void OnAiStateTransition(AiState next);
    [Serializable]
    public abstract class AiState : ScriptableObject, IAiGoal
    {
        [SerializeField] protected float m_basePriority = default;
        public event OnAiStateTransition OnTransition;
        public event OnActionFinish OnFinish;
        public virtual bool PreCondition(NpcWorldState args) 
        {
            return true;
        }

        public virtual float Priority(NpcWorldState args) 
        {
            return m_basePriority;
        }

        public abstract void OnEnter(NpcController controller, NpcWorldState state);
        public abstract void OnUpdate(NpcController controller, NpcWorldState state);
        public virtual void TransitionTo(AiState next) 
        {
            OnTransition?.Invoke(next);
        }

        protected virtual void OnActionFinished() 
        {
            OnFinish?.Invoke();
        }
    }
}
