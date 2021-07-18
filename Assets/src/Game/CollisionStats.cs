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

        public CollisionStats(float contactDamage, float knockback) 
        {
            m_contactDamage = contactDamage;
            m_knockback = knockback;
        }
    }
}
