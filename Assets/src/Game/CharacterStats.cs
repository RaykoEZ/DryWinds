using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public class CharacterStats 
    {
        [SerializeField] protected float m_maxStamina = default;
        [SerializeField] protected float m_maxSp = default;
        [SerializeField] protected float m_stamina = default;
        [SerializeField] protected float m_sp = default;
        [SerializeField] protected float m_speed = default;
        [SerializeField] protected float m_spRegenPerSec = default;

        protected bool m_isDirty = false;
        public bool IsDirty { get { return m_isDirty; } }

        public float MaxStamina { get { return m_maxStamina; } set { m_maxStamina = value; m_isDirty = true; } }
        public float MaxSP { get { return m_maxSp; } set { m_maxSp = value; m_isDirty = true; } }
        public float Stamina { get { return m_stamina; } set { m_stamina = value; m_isDirty = true; } }
        public float SP { get { return m_sp; } set { m_sp = value; m_isDirty = true; } }
        public float Speed { get { return m_speed; } set { m_speed = value; m_isDirty = true; } }
        public float SPRegenPerSec { get { return m_spRegenPerSec; } set { m_spRegenPerSec = value; m_isDirty = true; } }

    }
}
