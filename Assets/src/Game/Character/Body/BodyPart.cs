using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    [RequireComponent(typeof(Collider2D))]
    public class BodyPart : MonoBehaviour
    {
        [SerializeField] protected Animator m_weaknessAnim = default;
        [SerializeField] protected bool m_inateBreakable = default;
        [SerializeField] protected int m_breakThreshold = default;
        [SerializeField] protected float m_hitModifier = default;
        protected bool m_breakable = false;
        protected int m_breakPoint = 0;
        protected int m_numWeakpointBreaks = 0;
        public event OnBodyHit OnBodyHit;

        protected virtual bool Breakable
        {
            get { return m_breakable; }
            set { m_breakable = value; }
        }

        protected virtual void Awake()
        {
            m_breakable = m_inateBreakable;
        }

        protected virtual void OnEnable() 
        {
            ResetProgress();
        }

        protected virtual void ResetProgress() 
        {
            m_numWeakpointBreaks = 0;
            m_breakPoint = 0;
        }

        public virtual void Hit(
            float damage, 
            float kbMod, Vector2 source,
            int partDamage = 1,
            List<CharacterModifier> modifiers = null) 
        {
            float resultDamage = m_hitModifier * damage;
            float knockback = Mathf.Clamp(kbMod - m_hitModifier, 0f, float.MaxValue);
            // damage this bodypart
            DamageBodyPart(partDamage, out bool partBreak, out bool weakBreak);
            BodyHitResult result = new BodyHitResult(
                partBreak, 
                weakBreak,
                resultDamage, partDamage,
                knockback, source,
                this,
                modifiers);
            // send hit result to manager
            OnBodyHit?.Invoke(result);
        }

        protected void DamageBodyPart(int damage, out bool partBreak, out bool weakBreak) 
        {
            partBreak = false;
            weakBreak = false;
            // if bodypart is breakable,
            // take part damage and show weakness if part HP depletes
            if (Breakable) 
            {
                m_breakPoint += damage;
                if (m_breakPoint > m_breakThreshold)
                {
                    m_breakPoint = 0;
                    ShowWeakpoint();
                    partBreak = true;
                }
            }
            // If part takes hit when weakpoint is showing, weakpoint breaks and expires
            else if (m_weaknessAnim.GetBool("Show"))
            {
                ++m_numWeakpointBreaks;
                HideWeakpoint();
                weakBreak = true;
            }
        }

        public void ShowWeakpoint() 
        {
            if (Breakable) 
            {
                Breakable = false;
                m_weaknessAnim?.SetBool("Show", true);
                StartCoroutine(BodyPartCooldown());
            }
        }

        public void HideWeakpoint() 
        {
            m_weaknessAnim?.SetBool("Show", false);
        }

        protected IEnumerator BodyPartCooldown() 
        {
            yield return new WaitForSeconds(10f);
            HideWeakpoint();
            Breakable = true;
            ResetProgress();
        }
    }
}
