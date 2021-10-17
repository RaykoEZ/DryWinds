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
    public abstract class AiAction<T> : ScriptableObject where T : IActionInput
    {
        [SerializeField] protected float m_basePriority = default;
        // Returns action executed
        public abstract ICharacterAction<T> Execute(NpcController controller, AiWorldState state);
       
        public virtual bool PreCondition(AiWorldState args)
        {
            return true;
        }

        public virtual float Priority(AiWorldState args) 
        {
            return m_basePriority;
        }
    
        protected virtual BaseCharacter ChooseTarget(List<BaseCharacter> characters) 
        {
            return HeuristicUtil.WeakestCharacter(characters);
        }

    }
}
