using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public class CharacterContext : IGameContext
    {
        [SerializeField] protected CollisionStats m_collisionStats = default;
        [SerializeField] protected CharacterStats m_characterStats = default;
        protected bool m_isDirty = false;

        public bool IsDirty
        {
            get
            {
                return m_isDirty ||
                    CharacterStats.IsDirty ||
                    m_collisionStats.IsDirty;
            }
        }

        #region Character Stats Properties
        public CollisionStats CollisionStats
        {
            get { return m_collisionStats; }
            set { m_collisionStats = value; m_isDirty = true; }
        }
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

        public CharacterContext(CharacterContext c)
        {
            m_collisionStats = new CollisionStats(c.m_collisionStats);
            m_characterStats = new CharacterStats(c.m_characterStats);
        }

        public CharacterContext(CharacterStats character, CollisionStats collision)
        {
            m_characterStats = new CharacterStats(character);
            m_collisionStats = new CollisionStats(collision);
        }

        public static CharacterContext operator +(CharacterContext a, CharacterContext b) 
        {
            return new CharacterContext(
                a.m_characterStats + b.m_characterStats, 
                a.m_collisionStats + b.m_collisionStats);
        }

        public static CharacterContext operator -(CharacterContext a, CharacterContext b)
        {
            return new CharacterContext(
                a.m_characterStats - b.m_characterStats,
                a.m_collisionStats - b.m_collisionStats);
        }

        public static CharacterContext operator *(CharacterContext a, CharacterContext mult)
        {
            return new CharacterContext(
                a.m_characterStats * mult.m_characterStats,
                a.m_collisionStats * mult.m_collisionStats);
        }

        public static CharacterContext operator /(CharacterContext a, CharacterContext div)
        {
            return new CharacterContext(
                a.m_characterStats / div.m_characterStats,
                a.m_collisionStats / div.m_collisionStats);
        }
    }


}
