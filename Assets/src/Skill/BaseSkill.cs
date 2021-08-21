using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    [Flags]
    public enum TargetOptions 
    { 
        None = 0,
        Self = 1,
        Ally = 1 << 1,
        Enemy = 1 << 2,
    }

    public class SkillTargetParam
    {
        Vector2 m_targetPos = default;

        public Vector2 TargetPos { get { return m_targetPos; } }

        public SkillTargetParam(Vector2 pos) 
        {
            m_targetPos = pos;
        }
    }

    public abstract class BaseSkill : MonoBehaviour
    {
        [SerializeField] protected Animator m_animator = default;
        [SerializeField] protected Collider2D m_hitBox = default;
        [SerializeField] protected SkillProperty m_skillProperty = default;

        protected bool m_onCD = false;
        protected bool m_isWindingUp = false;
        protected bool m_skillActive = false;
        protected float m_windupTimer = 0f;
        protected BaseCharacter m_user = default;
        protected Coroutine m_currentSkill = default;
        protected Coroutine m_currentWindup = default;

        public SkillProperty SkillProperties { get { return m_skillProperty; } }
        public float MaxWindUpTime { get { return m_skillProperty.MaxWindupTime; } }

        public virtual bool SkillUsable
        {
            get
            {
                return !m_onCD &&
                    m_user?.CurrentStats.SP >= m_skillProperty.SpCost;
            }
        }
        protected abstract IEnumerator SkillEffect(SkillTargetParam target = null);

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

        public virtual void Init(BaseCharacter user, bool hitBoxOn = false) 
        {
            m_user = user;
            m_hitBox.enabled = hitBoxOn;
        }

        public virtual void SkillWindup()
        {
            if(!SkillUsable || m_skillProperty.MaxWindupTime == 0) 
            {
                return;
            }

            // reset windup timer if player charges again before skill activation
            if(m_currentWindup != null) 
            {
                m_windupTimer = 0f;
                StopCoroutine(m_currentWindup);
            }

            m_isWindingUp = true;
            m_currentWindup = StartCoroutine(OnWindup());
        }

        // The logics and interactions of the skill on each target
        /// @param target: initial target for skill
        public virtual void Execute(SkillTargetParam target = null) 
        {
            m_isWindingUp = false;
            if (SkillUsable && m_user != null)
            {
                m_onCD = true;
                m_skillActive = true;
                m_user.CurrentStats.SP = Mathf.Max(0f, m_user.CurrentStats.SP - m_skillProperty.SpCost);
                StartCoroutine(OnCooldown());
                m_currentSkill = StartCoroutine(SkillEffect(target));
            }
        }

        public virtual void CancelWindup() 
        {
            if(m_currentWindup != null) 
            {
                StopCoroutine(m_currentWindup);
            }

            m_isWindingUp = false;
            m_windupTimer = 0f;
        }

        public virtual void EndSkillEffect()
        {
            m_skillActive = false;
        }

        protected virtual IEnumerator OnWindup() 
        { 
            while(m_isWindingUp && SkillUsable) 
            {
                m_windupTimer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            m_currentWindup = null;
        }

        protected virtual IEnumerator OnCooldown() 
        {
            //start cooldown and reset skill states
            yield return new WaitForSeconds(m_skillProperty.CooldownTime);
            m_onCD = false;
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
    }

}
