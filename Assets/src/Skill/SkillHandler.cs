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
        protected BaseCharacter m_userRef;

        public bool IsSkillAvailable
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

        protected BaseSkill CurrentSkill
        {
            get { return m_skills.EquippedSkill; }
        }

        public void Init(BaseCharacter user, bool hitBoxOn = false)
        {
            foreach(BaseSkill skill in m_skills.SkillList) 
            {
                skill.Init(user, hitBoxOn);
            }
        }

        public virtual void SkillWindup()
        {
            CurrentSkill.SkillWindup();
        }

        public virtual void ActivateSkill(Vector2 targetPos, Dictionary<string, object> payload = null)
        {
            SkillParam param = new SkillParam(targetPos, payload);
            CurrentSkill.Execute(param);
        }
        public virtual void ActivateSkill(SkillParam param)
        {
            CurrentSkill.Execute(param);
        }

        public virtual void InterruptSkill()
        {
            if (CurrentSkill.IsWindingUp)
            {
                CurrentSkill.CancelWindup();
            }
            else
            {
                CurrentSkill.EndSkillEffect();
            }
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
