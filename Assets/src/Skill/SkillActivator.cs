using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class SkillActivator
    {
        public bool IsCurrentSkillAvailable
        {
            get
            {
                return CurrentSkill != null && CurrentSkill.IsUsable;
            }
        }

        public SkillProperty CurrentSkillProperties
        {
            get { return CurrentSkill.Properties; }
        }

        public ICharacterAction<IActionInput, SkillProperty> CurrentSkill
        {
            get; protected set;
        }

        public virtual void SkillWindup()
        {
            CurrentSkill?.Windup();
        }

        public virtual void EquipSkill(ICharacterAction<IActionInput, SkillProperty> skill) 
        {      
            CurrentSkill = skill;
        }

        public virtual void ActivateSkill(ITargetable<Vector2> target, bool isDirection = false, Dictionary<string, object> payload = null)
        {           
            VectorInput param = new VectorInput(target, payload);
            CurrentSkill?.Execute(param);
        }

        public virtual void ActivateSkill(ITargetable<BaseCharacter> target, bool isDirection = false, Dictionary<string, object> payload = null)
        {
            CharacterParam param = new CharacterParam(target, payload);
            CurrentSkill?.Execute(param);
        }

        public virtual void InterruptSkill()
        {
            CurrentSkill?.Interrupt();
        }
    }
}
