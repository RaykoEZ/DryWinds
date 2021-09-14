using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    // A container class for skill assets
    [Serializable]
    public class SkillInventory
    {
        protected int m_equippedIndex = 0;
        [SerializeField] protected List<BaseSkill> m_skillList = default;

        public List<BaseSkill> SkillList { get { return m_skillList; } }

        public int EquippedTraceIndex { 
            get { return m_equippedIndex; } 
            set { m_equippedIndex = Mathf.Clamp(value, 0, m_skillList.Count - 1); } }

        public BaseSkill EquippedSkill 
        { get 
            { 
                return m_skillList[EquippedTraceIndex];
            } 
        }

        public BaseSkill GetSkill(int index)
        {
            if (index >= m_skillList.Count || index < 0) 
            {
                return null;
            }

            return m_skillList[index];
        }

        public void AddSkill(BaseSkill skill) 
        {
            m_skillList.Add(skill);
        }
    }
}
