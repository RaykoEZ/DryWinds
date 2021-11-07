using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/BasicAttack", order = 0)]
    public class AiBasicAttack : AiSkill
    {
        public override bool PreCondition(NpcWorldState args)
        {

            bool baseCheck = base.PreCondition(args);
            if (!baseCheck) 
            {
                return baseCheck;
            }

            BaseCharacter target = ChooseTarget(args.Enemies);
            ICharacterAction<IActionInput, SkillProperty> skill = ChooseAction(args.BasicSkills, target);

            return baseCheck && skill != null;
        }

        protected override BaseCharacter ChooseTarget(List<BaseCharacter> characters)
        {
            return HeuristicUtil.WeakestCharacter(characters);
        }

        protected override float ActionScore(ICharacterAction<IActionInput, SkillProperty> action, BaseCharacter target)
        {
            float ret = 0f;
            if (!action.IsUsable )
            {
                return ret;
            }

            SkillProperty properties = action.Properties;
            ret = (properties.SkillValue - 0.1f * properties.MaxWindupTime) / properties.SpCost;
            // base damage score
            return ret;
        }
    }
}
