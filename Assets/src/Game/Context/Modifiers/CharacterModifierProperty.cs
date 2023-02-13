using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public interface IModifierProperty
    {
    }

    [Serializable]
    public struct CharacterModifierProperty : IModifierProperty
    {
        [SerializeField] float m_maxStamina;
        [SerializeField] float m_maxSp;
        [SerializeField] float m_speed;
        [SerializeField] float m_spRegenPerSec;
        [SerializeField] float m_hitRecoveryTime;

        public float MaxStamina { get { return m_maxStamina;} }
        public float MaxSP { get { return m_maxSp;} }
        public float Speed { get { return m_speed;} }
        public float SPRegenPerSec { get { return m_spRegenPerSec;} }
        public float HitRecoveryTime { get { return m_hitRecoveryTime;} }

        public CharacterModifierProperty(float value) 
        {
            m_maxStamina = value;
            m_maxSp = value;
            m_speed = value;
            m_spRegenPerSec = value;
            m_hitRecoveryTime = value;
        }

        public CharacterModifierProperty(
            float maxStam,
            float maxSp,
            float spd,
            float spRegen,
            float hitRecover
            )
        {
            m_maxStamina = maxStam;
            m_maxSp = maxSp;
            m_speed = spd;
            m_spRegenPerSec = spRegen;
            m_hitRecoveryTime = hitRecover;
        }

        #region operators

        public static CharacterModifierProperty operator +(CharacterModifierProperty a, CharacterModifierProperty b)
        {
            CharacterModifierProperty ret = new CharacterModifierProperty
            {
                m_maxStamina = a.m_maxStamina + b.m_maxStamina,
                m_maxSp = a.m_maxSp + b.m_maxSp,
                m_speed = a.m_speed + b.m_speed,
                m_spRegenPerSec = a.m_spRegenPerSec + b.m_spRegenPerSec,
                m_hitRecoveryTime = a.m_hitRecoveryTime + b.m_hitRecoveryTime
            };
            return ret;
        }

        public static CharacterModifierProperty operator -(CharacterModifierProperty a, CharacterModifierProperty b)
        {
            CharacterModifierProperty ret = new CharacterModifierProperty
            {
                m_maxStamina = a.m_maxStamina - b.m_maxStamina,
                m_maxSp = a.m_maxSp - b.m_maxSp,
                m_speed = a.m_speed - b.m_speed,
                m_spRegenPerSec = a.m_spRegenPerSec - b.m_spRegenPerSec,
                m_hitRecoveryTime = a.m_hitRecoveryTime - b.m_hitRecoveryTime
            };
            return ret;
        }

        public static CharacterModifierProperty operator *(CharacterModifierProperty a, CharacterModifierProperty b)
        {
            CharacterModifierProperty ret = new CharacterModifierProperty
            {
                m_maxStamina = a.m_maxStamina * b.m_maxStamina,
                m_maxSp = a.m_maxSp * b.m_maxSp,
                m_speed = a.m_speed * b.m_speed,
                m_spRegenPerSec = a.m_spRegenPerSec * b.m_spRegenPerSec,
                m_hitRecoveryTime = a.m_hitRecoveryTime * b.m_hitRecoveryTime
            };
            return ret;
        }

        public static CharacterModifierProperty operator /(CharacterModifierProperty a, CharacterModifierProperty b)
        {
            CharacterModifierProperty ret = new CharacterModifierProperty
            {
                m_maxStamina = Mathf.Approximately(b.m_maxStamina, 0f) ? a.m_maxStamina : a.m_maxStamina / b.m_maxStamina,
                m_maxSp = Mathf.Approximately(b.m_maxSp, 0f) ? a.m_maxSp : a.m_maxSp / b.m_maxSp,
                m_speed = Mathf.Approximately(b.m_speed, 0f) ? a.m_speed : a.m_speed / b.m_speed,
                m_spRegenPerSec = Mathf.Approximately(b.m_spRegenPerSec, 0f) ? a.m_spRegenPerSec : a.m_spRegenPerSec / b.m_spRegenPerSec,
                m_hitRecoveryTime = Mathf.Approximately(b.m_hitRecoveryTime, 0f) ? a.m_hitRecoveryTime : a.m_hitRecoveryTime / b.m_hitRecoveryTime
            };
            return ret;
        }

        #endregion
    }
}
