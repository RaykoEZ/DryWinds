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
        [SerializeField] protected float m_hitRecoveryTime = default;
        [SerializeField] protected CollisionStats m_collisionStats = default;

        protected bool m_isDirty = false;
        public bool IsDirty { get { return m_isDirty || m_collisionStats.IsDirty; } }

        public float MaxStamina { get { return m_maxStamina; } set { m_maxStamina = Mathf.Max(1f, value); m_isDirty = true; } }
        public float MaxSP { get { return m_maxSp; } set { m_maxSp = Mathf.Max(0f, value); m_isDirty = true; } }
        public float Stamina { get { return m_stamina; } set { m_stamina = Mathf.Min(MaxStamina, value); m_isDirty = true; } }
        public float SP { get { return m_sp; } set { m_sp = Mathf.Min(MaxSP, value); m_isDirty = true; } }
        public float Speed { get { return m_speed; } set { m_speed = Mathf.Clamp(value, 0f, 5f); m_isDirty = true; } }
        public float SPRegenPerSec { get { return m_spRegenPerSec; } set { m_spRegenPerSec = value; m_isDirty = true; } }
        public float HitRecoveryTime { get { return (m_hitRecoveryTime * MaxStamina) / (Stamina + 0.2f * MaxStamina); } set { m_hitRecoveryTime = value; m_isDirty = true; } }

        public CollisionStats CollisionStats
        {
            get { return m_collisionStats; }
            set { m_collisionStats = value; m_isDirty = true; }
        }
        public CharacterStats(CharacterStats stat)
        {
            m_maxStamina = stat.m_maxStamina;
            m_maxSp = stat.m_maxSp;
            m_stamina = stat.m_stamina;
            m_sp = stat.m_sp;
            m_speed = stat.m_speed;
            m_spRegenPerSec = stat.m_spRegenPerSec;
            m_hitRecoveryTime = stat.m_hitRecoveryTime;
            m_collisionStats = stat.m_collisionStats;
        }
        public CharacterStats(float val)
        {
            m_maxStamina = val;
            m_maxSp = val;
            m_stamina = val;
            m_sp = val;
            m_speed = val;
            m_spRegenPerSec = val;
            m_hitRecoveryTime = val;
            m_collisionStats = new CollisionStats(val);
        }
        public CharacterStats() 
        {
            m_maxStamina = 0f;
            m_maxSp = 0f;
            m_stamina = 0f;
            m_sp = 0f;
            m_speed = 0f;
            m_spRegenPerSec = 0f;
            m_hitRecoveryTime = 0f;
            m_collisionStats = new CollisionStats(0f);

        }

        #region Modifier Operators
        public static CharacterStats operator +(CharacterStats a, CharacterModifierProperty b) 
        {
            CharacterStats ret = new CharacterStats(a);
            ret.m_maxStamina += b.MaxStamina;
            ret.m_maxSp += b.MaxSP;
            ret.m_speed += b.Speed;
            ret.m_spRegenPerSec += b.SPRegenPerSec;
            ret.m_hitRecoveryTime += b.HitRecoveryTime;
            ret.m_collisionStats += new CollisionStats(b.ContactDamage, b.Knockback);
            return ret;
        }

        public static CharacterStats operator -(CharacterStats a, CharacterModifierProperty b)
        {
            CharacterStats ret = new CharacterStats(a);
            ret.m_maxStamina -= b.MaxStamina;
            ret.m_maxSp -= b.MaxSP;
            ret.m_speed -= b.Speed;
            ret.m_spRegenPerSec -= b.SPRegenPerSec;
            ret.m_hitRecoveryTime -= b.HitRecoveryTime;
            ret.m_collisionStats -= new CollisionStats(b.ContactDamage, b.Knockback);
            return ret;
        }

        public static CharacterStats operator *(CharacterStats a, CharacterModifierProperty b)
        {
            CharacterStats ret = new CharacterStats(a);
            ret.m_maxStamina *= b.MaxStamina;
            ret.m_maxSp *= b.MaxSP;
            ret.m_speed *= b.Speed;
            ret.m_spRegenPerSec *= b.SPRegenPerSec;
            ret.m_hitRecoveryTime *= b.HitRecoveryTime;
            ret.m_collisionStats *= new CollisionStats(b.ContactDamage, b.Knockback);
            return ret;
        }

        public static CharacterStats operator /(CharacterStats a, CharacterModifierProperty b)
        {
            CharacterStats ret = new CharacterStats(a);
            ret.m_maxStamina /= Mathf.Approximately(b.MaxStamina, 0f) ? 1f : b.MaxStamina;
            ret.m_maxSp /= Mathf.Approximately(b.MaxSP, 0f)? 1f : b.MaxSP;
            ret.m_speed /= Mathf.Approximately(b.Speed, 0f) ? 1f : b.Speed;
            ret.m_spRegenPerSec /= Mathf.Approximately(b.SPRegenPerSec, 0f) ? 1f : b.SPRegenPerSec;
            ret.m_hitRecoveryTime /= Mathf.Approximately(b.HitRecoveryTime, 0f) ? 1f : b.HitRecoveryTime;
            ret.m_collisionStats /= new CollisionStats
                (
                    b.ContactDamage,
                    b.Knockback
                );

            return ret;
        }

        #endregion
    }
}
