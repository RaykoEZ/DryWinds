using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public class CollisionStats 
    {
        [SerializeField] protected float m_contactDamage = default;
        [SerializeField] protected float m_knockback = default;

        protected bool m_isDirty = false;
        public bool IsDirty { get { return m_isDirty; } }
        public float ContactDamage { get { return m_contactDamage; } set { m_contactDamage = value; m_isDirty = true; } }
        public float Knockback { get { return m_knockback; } set { m_knockback = value; m_isDirty = true; } }
        public CollisionStats() 
        {
            m_contactDamage = 0f;
            m_knockback = 0f;
        }

        public CollisionStats(float contactDamage, float knockback) 
        {
            m_contactDamage = contactDamage;
            m_knockback = knockback;
        }

        public CollisionStats(float val)
        {
            m_contactDamage = val;
            m_knockback = val;
        }
        public CollisionStats(CollisionStats stats)
        {
            m_contactDamage = stats.ContactDamage;
            m_knockback = stats.Knockback;
        }

        #region Operators
        public static CollisionStats operator +(CollisionStats a, CollisionStats b) 
        {
            return new CollisionStats(
                Mathf.Max(0f, a.m_contactDamage + b.m_contactDamage), Mathf.Max(0f, a.m_knockback + b.m_knockback));
        }
        public static CollisionStats operator -(CollisionStats a, CollisionStats b)
        {
            return new CollisionStats(
                Mathf.Max(0f, a.m_contactDamage - b.m_contactDamage), Mathf.Max(0f, a.m_knockback - b.m_knockback));
        }
        public static CollisionStats operator *(CollisionStats a, CollisionStats b)
        {
            return new CollisionStats(
                Mathf.Max(0f, a.m_contactDamage * b.m_contactDamage), Mathf.Max(0f, a.m_knockback * b.m_knockback));
        }
        public static CollisionStats operator /(CollisionStats a, CollisionStats b)
        {
            return new CollisionStats(
                Mathf.Approximately(b.m_contactDamage, 0f) ? a.m_contactDamage : a.m_contactDamage / b.m_contactDamage,
                Mathf.Approximately(b.m_knockback, 0f) ? a.m_knockback : a.m_knockback / b.m_knockback);
        }
        #endregion
    }
}
