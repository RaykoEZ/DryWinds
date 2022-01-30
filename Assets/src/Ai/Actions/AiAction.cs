using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public delegate void OnAiActionFinish();
    [Serializable]
    public abstract class AiAction<T> : MonoBehaviour, ICharacterAction<AiActionInput> where T : IActionInput
    {
        [SerializeField] protected float m_basePriority = default;
        [SerializeField] protected float m_cooldownTime = default;
        // cooldown ends when internal execute coroutine finishes
        public bool OnCooldown { get { return false; }}
        public virtual bool IsUsable { get { return !OnCooldown; } }
        public virtual ActionProperty Properties { get { return m_prop; } }
        public event OnActionFinish<AiActionInput> OnFinish;
        protected ActionProperty m_prop = new ActionProperty();
        Coroutine m_execute = null;
        Coroutine m_executeInternal = null;

        public virtual void OnEnter(AiActionInput param) 
        {
            if (!OnCooldown) 
            {
                m_execute = StartCoroutine(ExecuteAction(param));
            }
        }

        IEnumerator ExecuteAction(AiActionInput param) 
        {
            yield return m_executeInternal = StartCoroutine(ExecuteInternal(param));
            OnActionFinish();
        }
        protected abstract IEnumerator ExecuteInternal(AiActionInput param);
        protected virtual void OnActionFinish() 
        {
            if (m_execute != null) 
            {
                StopCoroutine(m_execute);
            }
            if (m_executeInternal != null)
            {
                StopCoroutine(m_executeInternal);
            }
            m_execute = null;
            m_executeInternal = null;
            OnFinish?.Invoke(this);
        }

        public void Interrupt()
        {
            OnActionFinish();
        }
       
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
