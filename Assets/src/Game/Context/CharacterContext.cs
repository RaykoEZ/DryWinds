using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Curry.Game
{
    [Serializable]
    public class CharacterContext : IGameContext
    {
        [SerializeField] protected CharacterStats m_characterStats = default;
        [SerializeField] protected List<AssetReference> m_basicSkillRefs = default;
        [SerializeField] protected List<AssetReference> m_drawSkillRefs = default;

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

        public List<AssetReference> BasicSkillAssetRefs
        {
            get { return new List<AssetReference>(m_basicSkillRefs); }
            set
            {
                m_basicSkillRefs = value;
                m_isDirty = true;
            }
        }

        public List<AssetReference> DrawSkilllAssetRefs
        {
            get { return new List<AssetReference>(m_drawSkillRefs); }
            set
            {
                m_drawSkillRefs = value;
                m_isDirty = true;
            }
        }
        #endregion

        public CharacterContext() 
        {
            m_characterStats = new CharacterStats();
            m_basicSkillRefs = new List<AssetReference>();
            m_drawSkillRefs = new List<AssetReference>();
        }

        public CharacterContext(float val, 
            List<AssetReference> skills = null, 
            List<AssetReference> draws = null
            )
        {
            m_characterStats = new CharacterStats(val);
            m_basicSkillRefs = skills == null ? new List<AssetReference>() : skills;
            m_drawSkillRefs = draws == null ? new List<AssetReference>() : draws;
        }

        public CharacterContext(CharacterStats c, 
            List<AssetReference> skills = null,
             List<AssetReference> draws = null
            )
        {
            m_characterStats = new CharacterStats(c);
            m_basicSkillRefs = skills == null? new List<AssetReference>() : skills;
            m_drawSkillRefs = draws == null ? new List<AssetReference>() : draws;

        }

        public CharacterContext(CharacterContext c)
        {
            m_characterStats = new CharacterStats(c.m_characterStats);
            m_basicSkillRefs = new List<AssetReference>(c.m_basicSkillRefs);
            m_drawSkillRefs = new List<AssetReference>(c.m_drawSkillRefs);
        }

        public static CharacterContext operator +(CharacterContext a, CharacterModifierProperty b) 
        {
            return new CharacterContext(a.CharacterStats + b, a.m_basicSkillRefs, a.m_drawSkillRefs);
        }

        public static CharacterContext operator -(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats - b, a.m_basicSkillRefs, a.m_drawSkillRefs);
        }

        public static CharacterContext operator *(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats * b, a.m_basicSkillRefs, a.m_drawSkillRefs);
        }

        public static CharacterContext operator /(CharacterContext a, CharacterModifierProperty b)
        {
            return new CharacterContext(a.CharacterStats / b, a.m_basicSkillRefs, a.m_drawSkillRefs);
        }
    }
}
