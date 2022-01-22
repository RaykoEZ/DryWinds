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
    public abstract class AiAction<T> : MonoBehaviour, ICharacterAction<AiActionInput> where T : IActionInput
    {
        [SerializeField] protected float m_basePriority = default;
        public virtual bool IsUsable { get { return true; } }
        public abstract bool ActionInProgress { get; protected set; }
        public virtual ActionProperty Properties { get { return m_prop; } }
        public event OnActionFinish<AiActionInput> OnFinish;
        protected ActionProperty m_prop = new ActionProperty();
        public abstract void Execute(AiActionInput param);
        public virtual void Interrupt()
        {
            ActionInProgress = false;
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
