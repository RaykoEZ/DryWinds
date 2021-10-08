using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;

namespace Curry.Ai
{
    public abstract class AiSkill : AiAction<IActionInput, SkillProperty>
    {
        public override bool PreCondition(NpcController controller, NpcWorldState args)
        {
            return args.Enemies.Count > 0 && args.BasicSkills.Count > 0;
        }

        public override ICharacterAction<IActionInput, SkillProperty> Execute(NpcController controller, NpcWorldState state)
        {
            BaseCharacter target = ChooseTarget(state.Enemies);
            ICharacterAction<IActionInput, SkillProperty> skill = ChooseAction(state.BasicSkills, target);
            ActivateSkill(controller, skill, target);
            return skill;
        }

        protected virtual void ActivateSkill(NpcController controller, ICharacterAction<IActionInput, SkillProperty> skill, BaseCharacter target) 
        {
            controller.EquipBasicSkill(skill);
            if(skill.Properties.MaxWindupTime > 0f) 
            {
                controller.OnSkillWindup(target);
            }
            else 
            {
                TargetPosition pos = new TargetPosition(target.transform.position);
                controller.OnBasicSkill(pos);
            }
        }

        public override ICharacterAction<IActionInput, SkillProperty> ChooseAction(
            List<ICharacterAction<IActionInput, SkillProperty>> skills, 
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

        protected abstract BaseCharacter ChooseTarget(List<BaseCharacter> characters);
    }
}
