using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class SkillActivator
    {
        SkillInventory m_skillSetRef = default;

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
            get { return m_skillSetRef.Skills; }
        }
        protected BaseSkill CurrentSkill
        {
            get { return m_skillSetRef.EquippedSkill; }
        }

        public void Init(BaseCharacter user, SkillInventory preparedSkills, bool hitBoxOn = false)
        {
            m_skillSetRef = preparedSkills;
            foreach (BaseSkill skill in m_skillSetRef.Skills) 
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

        //skill swap ops
        public virtual void ChangeSkill(int index)
        {
            m_skillSetRef.EquippedTraceIndex = index;
        }
        public virtual void NextSkill()
        {
            m_skillSetRef.EquippedTraceIndex++;
        }

        public virtual void PreviousSkill()
        {
            m_skillSetRef.EquippedTraceIndex--;
        }
    }
}
