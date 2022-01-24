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
        // cooldown ends when internal execute coroutine finishes
        public bool OnCooldown { get { return m_execute != null; }}
        public virtual bool IsUsable { get { return OnCooldown; } }
        public virtual ActionProperty Properties { get { return m_prop; } }
        public event OnActionFinish<AiActionInput> OnFinish;
        protected ActionProperty m_prop = new ActionProperty();
        protected Coroutine m_execute;
        public virtual void Execute(AiActionInput param) 
        {
            m_execute = StartCoroutine(ExecuteInternal(param));
        }

        protected abstract IEnumerator ExecuteInternal(AiActionInput param); 

        public virtual void Interrupt()
        {
            if(m_execute != null) 
            {
                StopCoroutine(m_execute);
            }
            OnFinish?.Invoke(this);
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
