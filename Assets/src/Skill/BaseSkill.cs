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

    [Serializable]
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
        [SerializeField] protected TargetOptions m_targetOptions = default;
        [SerializeField] protected float m_cooldownTime = default;
        [SerializeField] protected int m_maxUseCount = default;
        [SerializeField] protected float m_maxWindupTime = default;
        [SerializeField] protected float m_spCost = default;

        protected bool m_onCD = false;
        protected bool m_isWindingUp = false;
        protected int m_currentUses = 0;
        protected float m_windupTimer = 0f;
        protected BaseCharacter m_user = default;
        protected Coroutine m_currentSkill = default;
        protected Coroutine m_currentWindup = default;

        public virtual bool SkillUsable
        {
            get
            {
                return m_currentUses > 0 && 
                    m_user?.CurrentStats.SP >= m_spCost;
            }
        }

        protected abstract IEnumerator SkillEffect(SkillTargetParam target = null);

        public virtual void Init(BaseCharacter user, bool hitBoxOn = false) 
        {
            m_user = user;
            m_hitBox.enabled = hitBoxOn;
            m_currentUses = m_maxUseCount;
        }

        public virtual void SkillWindup()
        {
            if(m_maxWindupTime == 0 || m_windupTimer >= m_maxWindupTime) 
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
        public virtual void Activate(SkillTargetParam target = null) 
        {
            m_isWindingUp = false;
            if (SkillUsable && m_user != null)
            {
                m_onCD = true;
                m_currentUses = Mathf.Max(0, --m_currentUses);
                m_user.CurrentStats.SP = Mathf.Max(0f, m_user.CurrentStats.SP - m_spCost);
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

        public virtual void InterruptSkill()
        {
            if(m_currentSkill != null) 
            {
                StopCoroutine(m_currentSkill);
            }
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
            yield return new WaitForSeconds(m_cooldownTime);
            m_currentUses = Mathf.Min(++m_currentUses, m_maxUseCount);
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
