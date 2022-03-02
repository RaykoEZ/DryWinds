using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
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
        public event OnBodyPartHit OnBodyPartBreak;
        public event OnBodyPartHit OnWeakpointBreak;
        public event OnCharacterTakeDamage OnTakeDamage;
        public virtual bool Breakable
        {
            get { return m_breakable; }
            protected set { m_breakable = value; }
        }

        protected virtual void Awake()
        {
            m_breakable = m_inateBreakable;
        }

        public void TakeDamage(float damage, int partDamage = 1) 
        {
            OnTakeDamage?.Invoke(m_hitModifier * damage);
            DamageBodyPart(partDamage);
        }

        protected void DamageBodyPart(int damage) 
        {
            if (Breakable) 
            {
                m_breakPoint += damage;
                if (m_breakPoint > m_breakThreshold)
                {
                    m_breakPoint = 0;
                    OnBodyPartBreak?.Invoke(this);
                    ShowWeakpoint();
                }
            }
            else if (m_weaknessAnim.GetBool("Show"))
            {
                OnWeakpointBreak(this);
                HideWeakpoint();
            }
        }

        public void ShowWeakpoint() 
        {
            if (Breakable) 
            {
                Breakable = false;
                m_weaknessAnim.SetBool("Show", true);
            }
        }

        public void HideWeakpoint() 
        {
            m_weaknessAnim.SetBool("Show", false);
        }
    }
}
