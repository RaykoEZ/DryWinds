using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class SkillInventoryManager : MonoBehaviour
    {
        [SerializeField] SkillInventory m_drawSkills = default;
        [SerializeField] SkillInventory m_basicSkills = default;

        public Asset CurrentDrawSkill { get { return m_drawSkills.EquippedSkill; } }
        public Asset CurrentBasicSkill { get { return m_basicSkills.EquippedSkill; } }

        // Draw skill ops
        public Asset ChangeDrawSkill(int index)
        {
            m_drawSkills.EquippedTraceIndex = index;
            return CurrentDrawSkill;
        }
        public Asset NextDrawSkill()
        {
            m_drawSkills.EquippedTraceIndex++;
            return CurrentDrawSkill;
        }

        public Asset PreviousDrawSkill()
        {
            m_drawSkills.EquippedTraceIndex--;
            return CurrentDrawSkill;
        }


        // Basic skill ops
        public Asset ChangeBasicSkill(int index)
        {
            m_basicSkills.EquippedTraceIndex = index;
            return CurrentBasicSkill;
        }
        public Asset NextBasicSkill()
        {
            m_basicSkills.EquippedTraceIndex++;
            return CurrentBasicSkill;
        }

        public Asset PreviousBasicSkill()
        {
            m_basicSkills.EquippedTraceIndex--;
            return CurrentBasicSkill;
        }
    }
}
