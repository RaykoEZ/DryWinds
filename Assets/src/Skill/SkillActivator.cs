using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class SkillActivator
    {
        public ICharacterAction<IActionInput> CurrentSkill
        {
            get; set;
        }

        public virtual void ActivateSkill(Vector2 target, bool isDirection = false, Dictionary<string, object> payload = null)
        {
            VectorInput param = new VectorInput(target, payload);
            CurrentSkill?.OnEnter(param);
        }

        public virtual void InterruptSkill()
        {
            CurrentSkill?.Interrupt();
        }
    }
}
