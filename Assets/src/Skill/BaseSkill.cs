using System;
using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public abstract class BaseSkill : MonoBehaviour, ICharacterAction<SkillParam>
    {
        [SerializeField] protected Animator m_animator = default;
        [SerializeField] protected SkillProperty m_skillProperty = default;

        public event OnActionFinish OnFinish;

        protected bool m_onCD = false;
        protected float m_windupTimer = 0f;
        protected BaseCharacter m_user = default;
        protected Coroutine m_currentSkill = default;
        protected Coroutine m_currentWindup = default;
        protected Coroutine m_coolDown = default;

        public SkillProperty SkillProperties { get { return m_skillProperty; } }
        protected bool IsWindingUp { get; set; }
        public bool ActionInProgress { get; protected set; }

        public virtual bool SkillUsable
        {
            get
            {
                return !m_onCD &&
                    m_user?.CurrentStats.SP >= m_skillProperty.SpCost;
            }
        }
        protected abstract IEnumerator SkillEffect(SkillParam target);

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

            if (hit == null || (hit.Relations & m_skillProperty.TargetOptions) == ObjectRelations.None)
            {
                return;
            }
            OnHit(hit);
        }

        public virtual void OnHit(Interactable hit) 
        {      
        }

        public virtual void Init(BaseCharacter user) 
        {
            m_user = user;
        }

        public virtual void SkillWindup()
        {
            if (!SkillUsable || m_skillProperty.MaxWindupTime == 0) 
            {
                return;
            }

            // reset windup timer if player charges again before skill activation
            if (m_currentWindup != null) 
            {
                m_windupTimer = 0f;
                StopCoroutine(m_currentWindup);
            }

            IsWindingUp = true;
            m_currentWindup = StartCoroutine(OnWindup());
        }

        // The logics and interactions of the skill on each target
        /// @param target: initial target for skill
        public virtual void Execute(SkillParam param)
        {
            IsWindingUp = false;
            if (SkillUsable && m_user != null)
            {
                ActionInProgress = true;
                ConsumeResource(m_skillProperty.SpCost);
                CoolDown();
                m_currentSkill = StartCoroutine(SkillEffect(param));
            }
        }
        public virtual void Interrupt()
        {
            if (IsWindingUp)
            {
                CancelWindup();
            }
            else
            {
                OnSkillFinish();
            }
        }
        public virtual void StartIframe()
        {
            int iframeLayer = LayerMask.NameToLayer("IFrame");
            m_user.gameObject.layer = iframeLayer;
        }

        public virtual void StopIframe()
        {
            // Set user back to default layer to collide with other characters
            m_user.gameObject.layer = 0;
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

        protected virtual void CancelWindup() 
        {
            if(m_currentWindup != null) 
            {
                StopCoroutine(m_currentWindup);
            }

            IsWindingUp = false;
            m_windupTimer = 0f;
        }

        protected virtual void OnSkillFinish() 
        {
            m_windupTimer = 0f;
            ActionInProgress = false;
            OnFinish?.Invoke();
        }

        protected virtual IEnumerator OnWindup() 
        { 
            while(IsWindingUp && SkillUsable) 
            {
                m_windupTimer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            m_currentWindup = null;
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
