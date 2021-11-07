using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    [Serializable]
    public class SkillHandler
    {
        [SerializeField] SkillInventory m_skills = default;

        public bool IsCurrentSkillAvailable
        {
            get
            {
                return CurrentSkill.SkillUsable;
            }
        }

        public SkillProperty CurrentSkillProperties
        {
            get { return CurrentSkill.SkillProperties; }
        }

        public List<BaseSkill> AllSkills
        {
            get { return m_skills.Skills; }
        }
        protected BaseSkill CurrentSkill
        {
            get { return m_skills.EquippedSkill; }
        }

        public SkillHandler(List<BaseSkill> preparedSkills)
        {
            m_skills = new SkillInventory(preparedSkills);
        }

        public void Init(BaseCharacter user, bool hitBoxOn = false)
        {
            foreach(BaseSkill skill in m_skills.Skills) 
            {
                skill.Init(user, hitBoxOn);
            }
        }

        public virtual void SkillWindup()
        {
            CurrentSkill.SkillWindup();
        }

        public virtual void ActivateSkill(ITargetable<Vector2> target, bool isDirection = false, Dictionary<string, object> payload = null)
        {           
            VectorParam param = new VectorParam(target, payload);
            CurrentSkill.Execute(param);
        }

        public virtual void ActivateSkill(ITargetable<BaseCharacter> target, bool isDirection = false, Dictionary<string, object> payload = null)
        {
            CharacterParam param = new CharacterParam(target, payload);
            CurrentSkill.Execute(param);
        }

        public virtual void ActivateSkill(SkillParam param)
        {
            CurrentSkill.Execute(param);
        }

        public virtual void InterruptSkill()
        {
            CurrentSkill.Interrupt();
        }

        public bool IsSkillAvailable(int index) 
        {
            if (index >= m_skills.Skills.Count) 
            {
                return false;
            }

            return m_skills.GetSkill(index).SkillUsable;
        }

        //skill swap ops
        public virtual void ChangeSkill(int index)
        {
            m_skills.EquippedTraceIndex = index;
        }
        public virtual void NextSkill()
        {
            m_skills.EquippedTraceIndex++;
        }

        public virtual void PreviousSkill()
        {
            m_skills.EquippedTraceIndex--;
        }
    }
}
