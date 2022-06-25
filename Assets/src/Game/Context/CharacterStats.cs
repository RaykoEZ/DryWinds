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
        [SerializeField] protected float m_moveSpeed = default;
        [SerializeField] protected float m_spRegenPerSec = default;
        [SerializeField] protected float m_hitRecoveryTime = default;
        protected bool m_isDirty = false;
        public bool IsDirty { get { return m_isDirty; } }

        public float MaxStamina { get { return m_maxStamina; } set { m_maxStamina = Mathf.Max(1f, value); m_isDirty = true; } }
        public float MaxSP { get { return m_maxSp; } set { m_maxSp = Mathf.Max(0f, value); m_isDirty = true; } }
        public float Stamina { get { return m_stamina; } set { m_stamina = Mathf.Clamp(value, 0f, MaxStamina); m_isDirty = true; } }
        public float SP { get { return m_sp; } set { m_sp = Mathf.Clamp(value, 0f, MaxSP); m_isDirty = true; } }
        public float MoveSpeed { get { return m_moveSpeed; } set { m_moveSpeed = Mathf.Clamp(value, 0f, 3f); m_isDirty = true; } }
        public float SPRegenPerSec { get { return m_spRegenPerSec; } set { m_spRegenPerSec = value; m_isDirty = true; } }
        public float HitRecoveryTime { get { return m_hitRecoveryTime * Mathf.Min(m_maxStamina/(m_stamina + 1f), 2f ); } set { m_hitRecoveryTime = value; m_isDirty = true; } }

        public CharacterStats(CharacterStats stat)
        {
            m_maxStamina = stat.m_maxStamina;
            m_maxSp = stat.m_maxSp;
            m_stamina = stat.m_stamina;
            m_sp = stat.m_sp;
            m_moveSpeed = stat.m_moveSpeed;
            m_spRegenPerSec = stat.m_spRegenPerSec;
            m_hitRecoveryTime = stat.m_hitRecoveryTime;
        }
        public CharacterStats(float val)
        {
            m_maxStamina = val;
            m_maxSp = val;
            m_stamina = val;
            m_sp = val;
            m_moveSpeed = val;
            m_spRegenPerSec = val;
            m_hitRecoveryTime = val;
        }
        public CharacterStats() 
        {
            m_maxStamina = 0f;
            m_maxSp = 0f;
            m_stamina = 0f;
            m_sp = 0f;
            m_moveSpeed = 0f;
            m_spRegenPerSec = 0f;
            m_hitRecoveryTime = 0f;
        }

        #region Modifier Operators
        public static CharacterStats operator +(CharacterStats a, CharacterModifierProperty b) 
        {
            CharacterStats ret = new CharacterStats(a);
            ret.m_maxStamina += b.MaxStamina;
            ret.m_maxSp += b.MaxSP;
            ret.m_moveSpeed += b.Speed;
            ret.m_spRegenPerSec += b.SPRegenPerSec;
            ret.m_hitRecoveryTime += b.HitRecoveryTime;
            return ret;
        }

        public static CharacterStats operator -(CharacterStats a, CharacterModifierProperty b)
        {
            CharacterStats ret = new CharacterStats(a);
            ret.m_maxStamina -= b.MaxStamina;
            ret.m_maxSp -= b.MaxSP;
            ret.m_moveSpeed -= b.Speed;
            ret.m_spRegenPerSec -= b.SPRegenPerSec;
            ret.m_hitRecoveryTime -= b.HitRecoveryTime;
            return ret;
        }

        public static CharacterStats operator *(CharacterStats a, CharacterModifierProperty b)
        {
            CharacterStats ret = new CharacterStats(a);
            ret.m_maxStamina *= b.MaxStamina;
            ret.m_maxSp *= b.MaxSP;
            ret.m_moveSpeed *= b.Speed;
            ret.m_spRegenPerSec *= b.SPRegenPerSec;
            ret.m_hitRecoveryTime *= b.HitRecoveryTime;
            return ret;
        }

        public static CharacterStats operator /(CharacterStats a, CharacterModifierProperty b)
        {
            CharacterStats ret = new CharacterStats(a);
            ret.m_maxStamina /= Mathf.Approximately(b.MaxStamina, 0f) ? 1f : b.MaxStamina;
            ret.m_maxSp /= Mathf.Approximately(b.MaxSP, 0f)? 1f : b.MaxSP;
            ret.m_moveSpeed /= Mathf.Approximately(b.Speed, 0f) ? 1f : b.Speed;
            ret.m_spRegenPerSec /= Mathf.Approximately(b.SPRegenPerSec, 0f) ? 1f : b.SPRegenPerSec;
            ret.m_hitRecoveryTime /= Mathf.Approximately(b.HitRecoveryTime, 0f) ? 1f : b.HitRecoveryTime;
            return ret;
        }

        #endregion
    }
}
