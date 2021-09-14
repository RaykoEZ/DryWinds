using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public class CharacterContext : IGameContext
    {
        [SerializeField] protected CharacterStats m_characterStats = default;
        protected bool m_isDirty = false;

        public bool IsDirty
        {
            get
            {
                return m_isDirty ||
                    CharacterStats.IsDirty;
            }
        }

        #region Character Stats Properties
        public CharacterStats CharacterStats
        {
            get
            {
                return m_characterStats;
            }
            set
            {
                m_characterStats = value;
                m_isDirty = true;
            }
        }
        #endregion

        public CharacterContext() 
        {
            m_characterStats = new CharacterStats();
        }

        public CharacterContext(float val)
        {
            m_characterStats = new CharacterStats(val);
        }
        public CharacterContext(CharacterStats c)
        {
            m_characterStats = new CharacterStats(c);
        }

        public CharacterContext(CharacterContext c)
        {
            m_characterStats = new CharacterStats(c.m_characterStats);
        }

        public static CharacterContext operator +(CharacterContext a, CharacterModifierProperty b) 
        {
            return new CharacterContext(a.CharacterStats + b);
        }

        public static CharacterContext operator -(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats - b);
        }

        public static CharacterContext operator *(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats * b);
        }

        public static CharacterContext operator /(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats / b);
        }
    }
}
