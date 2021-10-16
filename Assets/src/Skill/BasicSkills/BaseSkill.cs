using System;
using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public abstract class BaseSkill : MonoBehaviour, ICharacterAction<IActionInput>
    {
        [SerializeField] protected Animator m_animator = default;
        [SerializeField] protected ActionProperty m_skillProperty = default;

        public event OnActionFinish<IActionInput> OnFinish;

        protected bool m_onCD = false;
        protected BaseCharacter m_user = default;
        protected Coroutine m_currentSkill = default;
        protected Coroutine m_coolDown = default;

        public ActionProperty Properties { get { return m_skillProperty; } }
        public bool ActionInProgress { get; protected set; }

        public virtual bool IsUsable
        {
            get
            {
                return !m_onCD &&
                    m_user?.CurrentStats.SP >= m_skillProperty.SpCost;
            }
        }
        protected abstract IEnumerator SkillEffect(IActionInput target);

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            Interactable hit = col.gameObject.GetComponent<Interactable>();

            if(hit == null || (hit.Relations & m_skillProperty.TargetOptions) == ObjectRelations.None) 
            {
                return;
            }
            OnHit(hit);
        }

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            Interactable hit = col.gameObject.GetComponent<Interactable>();

            if (hit == null)
            {
                return;
            }

            bool isAlly = m_user.Relations == hit.Relations;
            if (m_skillProperty.TargetOptions == ObjectRelations.Ally &&
                isAlly) 
            {
                OnHit(hit);
                return;
            }

            if (m_skillProperty.TargetOptions == ObjectRelations.Enemy &&
                !isAlly)
            {
                OnHit(hit);
                return;
            }
        }

        public virtual void OnHit(Interactable hit) 
        {      
        }

        public virtual void Init(BaseCharacter user) 
        {
            m_user = user;
        }

        // The logics and interactions of the skill on each target
        /// @param target: initial target for skill
        public virtual void Execute(IActionInput param)
        {
            if (IsUsable && m_user != null)
            {
                ActionInProgress = true;
                ConsumeResource(m_skillProperty.SpCost);
                CoolDown();
                m_currentSkill = StartCoroutine(SkillEffect(param));
            }
        }
        public virtual void Interrupt()
        {
            OnSkillFinish();
        }

        protected virtual void CoolDown() 
        {
            if(m_coolDown == null) 
            {
                m_coolDown = StartCoroutine(OnCooldown());
            }
        }

        protected virtual void ConsumeResource(float val) 
        {
            m_user.OnLoseSp(val);
        }

        protected virtual void OnSkillFinish() 
        {
            ActionInProgress = false;
            OnFinish?.Invoke(this);
        }

        protected virtual IEnumerator OnCooldown() 
        {
            m_onCD = true;
            //start cooldown and reset skill states
            yield return new WaitForSeconds(m_skillProperty.CooldownTime);
            m_coolDown = null;
            m_onCD = false;
        }
    }

}
