using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public delegate void OnAiActionFinish();
    [Serializable]
    public abstract class AiAction<T> : MonoBehaviour where T : IActionInput
    {
        [SerializeField] protected float m_basePriority = default;
        [SerializeField] protected float m_cooldownTime = default;
        // cooldown ends when internal execute coroutine finishes
        public virtual bool OnCooldown { get { return false; }}
        public virtual bool IsUsable { get { return !OnCooldown; } }

        public virtual void OnEnter(AiActionInput param)
        {
            if (IsUsable) 
            {
                ExecuteAction(param);
            }
        }

        protected abstract void ExecuteAction(AiActionInput param);
       
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
