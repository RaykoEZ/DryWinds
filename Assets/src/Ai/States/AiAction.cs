using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;

namespace Curry.Ai
{
    public delegate void OnAiActionTransition(AiAction next);
    public delegate void OnAiActionFinish();
    [Serializable]
    public abstract class AiAction : ScriptableObject
    {
        [SerializeField] protected float m_basePriority = default;
        public event OnAiActionTransition OnTransition;
        public event OnAiActionFinish OnFinish;

        public virtual bool PreCondition(NpcWorldState args) 
        {
            return true;
        }

        public virtual float Priority(NpcWorldState args) 
        {
            return m_basePriority;
        }

        public abstract void Execute(NpcController controller, NpcWorldState state);
        public virtual void TransitionTo(AiAction next) 
        {
            OnTransition?.Invoke(next);
        }

        protected virtual void OnActionFinished<T>(ICharacterAction<T> action) where T : IActionParam 
        {
            // We consume the callback so we don't duplicate for every action execution
            action.OnFinish -= OnActionFinished;
            OnFinish?.Invoke();
        }
    }
}
