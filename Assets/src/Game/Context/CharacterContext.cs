using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    [Serializable]
    public class CharacterContext : IGameContext
    {
        [SerializeField] protected CharacterStats m_characterStats = default;
        [SerializeField] protected List<string> m_basicSkillNames = default;
        [SerializeField] protected List<string> m_drawSkillNames = default;

        protected bool m_isDirty = false;

        public bool IsDirty
        {
            get
            {
                return m_isDirty ||
                    CharacterStats.IsDirty;
            }
        }

        #region Character Properties
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

        public List<string> BasicSkillNames
        {
            get { return new List<string>(m_basicSkillNames); }
            set
            {
                m_basicSkillNames = value;
                m_isDirty = true;
            }
        }

        public List<string> DrawSkillNames
        {
            get { return new List<string>(m_drawSkillNames); }
            set
            {
                m_drawSkillNames = value;
                m_isDirty = true;
            }
        }
        #endregion

        public CharacterContext() 
        {
            m_characterStats = new CharacterStats();
            m_basicSkillNames = new List<string>();
            m_drawSkillNames = new List<string>();
        }

        public CharacterContext(float val, 
            List<string> skillNames = null, 
            List<string> drawNames = null
            )
        {
            m_characterStats = new CharacterStats(val);
            m_basicSkillNames = skillNames == null ? new List<string>() : skillNames;
            m_drawSkillNames = drawNames == null ? new List<string>() : drawNames;
        }

        public CharacterContext(CharacterStats c, 
            List<string> skillNames = null,
             List<string> drawNames = null
            )
        {
            m_characterStats = new CharacterStats(c);
            m_basicSkillNames = skillNames == null? new List<string>() : skillNames;
            m_drawSkillNames = drawNames == null ? new List<string>() : drawNames;

        }

        public CharacterContext(CharacterContext c)
        {
            m_characterStats = new CharacterStats(c.m_characterStats);
            m_basicSkillNames = new List<string>(c.m_basicSkillNames);
            m_drawSkillNames = new List<string>(c.m_drawSkillNames);
        }

        public static CharacterContext operator +(CharacterContext a, CharacterModifierProperty b) 
        {
            return new CharacterContext(a.CharacterStats + b, a.m_basicSkillNames, a.m_drawSkillNames);
        }

        public static CharacterContext operator -(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats - b, a.m_basicSkillNames, a.m_drawSkillNames);
        }

        public static CharacterContext operator *(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats * b, a.m_basicSkillNames, a.m_drawSkillNames);
        }

        public static CharacterContext operator /(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats / b, a.m_basicSkillNames, a.m_drawSkillNames);
        }
    }
}
