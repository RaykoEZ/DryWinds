using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;

namespace Curry.Ai
{
    public abstract class AiSkill : AiAction<IActionInput>
    {
        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count > 0 && args.BasicSkills.Count > 0;
        }

        protected override void ExecuteInternal(AiActionInput param)
        {
            BaseCharacter target = ChooseTarget(param.WorldState.Enemies);
            ICharacterAction<IActionInput> skill = ChooseAction(param.WorldState.BasicSkills, target);
            Debug.Log($"Using Skill: {skill.Properties.Name}");
            ActivateSkill(param.Controller, skill, target);
        }

        protected virtual void ActivateSkill(NpcController controller, ICharacterAction<IActionInput> skill, BaseCharacter target) 
        {
            controller.EquipBasicSkill(skill);
            controller.OnBasicSkill(target);
        }

        public virtual ICharacterAction<IActionInput> ChooseAction(
            List<ICharacterAction<IActionInput>> skills, 
            BaseCharacter target)
        {
            int bestIdx = 0;
            float highScore = 0f;

            for (int i = 0; i < skills.Count; ++i)
            {
                float score = ActionScore(skills[i], target);
                if (score > highScore)
                {
                    highScore = score;
                    bestIdx = i;
                }
            }

            if (Mathf.Approximately(highScore, 0f))
            {
                return null;
            }
            return skills[bestIdx];
        }

        protected abstract float ActionScore(ICharacterAction<IActionInput> action, BaseCharacter target);
    }
}
