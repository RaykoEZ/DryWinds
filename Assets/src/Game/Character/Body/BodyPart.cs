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
        [SerializeField] protected SpriteRenderer m_sprite = default;
        [SerializeField] protected bool m_inateBreakable = default;
        [SerializeField] protected int m_breakThreshold = default;
        [SerializeField] protected float m_weakDuration = default;
        [SerializeField] protected float m_defaultHitModifier = default;
        protected bool m_breakable = false;
        protected bool m_weaknessExposed = false;
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
            Hide();
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
            float resultDamage = m_defaultHitModifier * damage;
            float knockback = Mathf.Clamp(kbMod - m_defaultHitModifier, 0f, float.MaxValue);
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

        protected void DamageBodyPart(int bodyDamage, out bool partBreak, out bool weakBreak) 
        {
            partBreak = false;
            weakBreak = false;
            // if bodypart is breakable,
            // take part damage and show weakness if part HP depletes
            if (Breakable) 
            {
                m_breakPoint += bodyDamage;
                if (m_breakPoint > m_breakThreshold)
                {
                    m_breakPoint = 0;
                    ShowWeakpoint();
                    partBreak = true;
                }
            }
            // If part takes hit when weakpoint is showing, weakpoint breaks and expires
            else if (m_weaknessExposed)
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
                StartCoroutine(Show());
            }
        }

        public void HideWeakpoint() 
        {
            Hide();
        }

        protected IEnumerator ResetBodyPart() 
        {
            yield return new WaitForSeconds(10f);
            Breakable = true;
            Hide();
            ResetProgress();
        }
        protected IEnumerator Show()
        {
            Breakable = false;
            m_weaknessExposed = true;
            float t = 0f;
            float showDuration = 1f;
            while(t < showDuration) 
            {
                t += Time.deltaTime;
                m_sprite.color = new Color(
                    m_sprite.color.r,
                    m_sprite.color.g,
                    m_sprite.color.b,
                    m_sprite.color.a + (t/showDuration));
                yield return null;
            }
            yield return new WaitForSeconds(m_weakDuration);
            //hiding
            Hide();
            StartCoroutine(ResetBodyPart());
        }

        protected void Hide() 
        {
            m_weaknessExposed = false;
            m_sprite.color = new Color(
                m_sprite.color.r,
                m_sprite.color.g,
                m_sprite.color.b, 0f);
        }

    }
}
