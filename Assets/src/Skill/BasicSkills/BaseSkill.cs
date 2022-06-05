using System;
using System.Collections;
using UnityEngine;
using Curry.Game;
using Curry.Ai;

namespace Curry.Skill
{
    [RequireComponent(typeof(Animator))]
    public abstract class BaseSkill : MonoBehaviour, ICharacterAction<IActionInput>
    {
        [SerializeField] protected Animator m_animator = default;
        [SerializeField] protected InteractableInstanceManager m_instanceManager = default;
        [SerializeField] protected ActionProperty m_skillProperty = default;
        public event OnActionFinish<IActionInput> OnFinish;
        protected BaseCharacter m_user = default;
        protected Coroutine m_coolDown = default;
        protected Coroutine m_execute;

        public ActionProperty Properties { get { return m_skillProperty; } }
        public bool OnCooldown { get { return m_coolDown != null; } }       
        public virtual bool IsUsable
        {
            get
            {
                return !OnCooldown &&
                    m_user?.CurrentStats.SP >= m_skillProperty.SpCost;
            }
        }

        protected abstract IEnumerator ExecuteInternal(IActionInput target);

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            BodyPart part = col.gameObject.GetComponent<BodyPart>();
            if (part != null)
            {
                OnHit(part);
            }
        }

        protected virtual void OnHit(BodyPart part) 
        {
        }

        public virtual void Init(BaseCharacter user) 
        {
            m_user = user;
        }

        // The logics and interactions of the skill on each target
        /// @param target: initial target for skill
        public virtual void OnEnter(IActionInput param)
        {
            if (IsUsable && m_user != null)
            {
                ConsumeResource(m_skillProperty.SpCost);
                CoolDown();
                m_animator.SetTrigger("Start");
                m_execute = StartCoroutine(ExecuteInternal(param));
            }
        }
        public virtual void Interrupt()
        {
            OnSkillFinish();
            if( m_execute != null ) 
            {
                StopCoroutine(m_execute);
            }
        }

        protected virtual void CoolDown() 
        {
            if (m_coolDown == null) 
            {
                m_coolDown = StartCoroutine(Coolingdown());
            }          
        }

        protected virtual void ConsumeResource(float val) 
        {
            m_user.OnLoseSp(val);
        }

        protected virtual void OnSkillFinish() 
        {
            OnFinish?.Invoke(this);
        }

        protected virtual IEnumerator Coolingdown() 
        {
            //start cooldown and reset skill states
            yield return new WaitForSeconds(m_skillProperty.CooldownTime);
            m_coolDown = null;
        }
    }

}
