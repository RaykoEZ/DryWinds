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
        bool OnCooldown { get; set; }
        public virtual bool IsUsable { get { return !OnCooldown; } }

        void OnEnable()
        {
            OnCooldown = false;
        }

        public virtual void OnEnter(AiActionInput param)
        {
            ExecuteAction(param);
            StartCoroutine(Cooldown());
        }

        protected abstract void ExecuteAction(AiActionInput param);
       
        public virtual bool PreCondition(AiWorldState args)
        {
            return IsUsable;
        }

        public virtual float Priority(AiWorldState args) 
        {
            return m_basePriority;
        }
    
        protected virtual BaseCharacter ChooseTarget(List<BaseCharacter> characters) 
        {
            return HeuristicUtil.WeakestCharacter(characters);
        }

        IEnumerator Cooldown() 
        {
            OnCooldown = true;
            yield return new WaitForSeconds(m_cooldownTime);
            OnCooldown = false;
        }
    }
}
