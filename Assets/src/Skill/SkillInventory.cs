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
        [SerializeField] protected int m_equippedTraceIndex = 0;
        [SerializeField] protected List<Asset> m_skillList = default;

        public List<Asset> SkillList { get { return m_skillList; } }

        public int EquippedTraceIndex { 
            get { return m_equippedTraceIndex; } 
            set { m_equippedTraceIndex = Mathf.Clamp(value, 0, m_skillList.Count - 1); } }

        public Asset EquippedSkill 
        { get 
            { 
                return m_skillList[m_equippedTraceIndex]; 
            } 
        }

        public Asset GetSkill(int index)
        {
            if (index >= m_skillList.Count || index < 0) 
            {
                return null;
            }

            return m_skillList[index];
        }

        public void AddSkill(Asset skill) 
        {
            m_skillList.Add(skill);
        }
    }
}
