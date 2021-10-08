using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;

namespace Curry.Ai
{
    public delegate void OnAiActionFinish();
    [Serializable]
    public abstract class AiAction<T, T1> : ScriptableObject where T : IActionInput where T1 : IActionProperty
    {
        [SerializeField] protected float m_basePriority = default;

        public virtual bool PreCondition(NpcController controller, NpcWorldState args)
        {
            return true;
        }

        public virtual float Priority(NpcWorldState args) 
        {
            return m_basePriority;
        }

        // Returns action executed
        public abstract ICharacterAction<T, T1> Execute(NpcController controller, NpcWorldState state);

        public abstract ICharacterAction<T, T1> ChooseAction(List<ICharacterAction<T, T1>> skills, BaseCharacter target);
        protected abstract float ActionScore(ICharacterAction<T, T1> action, BaseCharacter target);
    }
}
