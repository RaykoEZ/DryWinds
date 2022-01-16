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

        public override ICharacterAction<IActionInput> Execute(NpcController controller, AiWorldState state)
        {
            Debug.Log("Skill");
            BaseCharacter target = ChooseTarget(state.Enemies);
            ICharacterAction<IActionInput> skill = ChooseAction(state.BasicSkills, target);
            ActivateSkill(controller, skill, target);
            return skill;
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
