using System;
using UnityEngine;
using Curry.Game;
using System.Collections;

namespace Curry.Ai
{
    [Serializable]
    [AddComponentMenu("Curry/Ai Action/Attack")]
    public class AiBasicAttack : AiSkill
    {
        public override bool PreCondition(AiWorldState args)
        {
            bool baseCheck = base.PreCondition(args);
            if (!baseCheck)
            {
                return baseCheck;
            }

            BaseCharacter target = ChooseTarget(args.Enemies);
            ICharacterAction<IActionInput> skill = ChooseAction(args.BasicSkills, target);

            return baseCheck && skill != null;
        }

        protected override float ActionScore(ICharacterAction<IActionInput> action, BaseCharacter target)
        {
            float ret = 0f;
            if (!action.IsUsable )
            {
                return ret;
            }

            ActionProperty properties = action.Properties;
            ret = (properties.ActionValue - 0.1f * properties.WindupTime) / properties.SpCost;
            // base damage score
            return ret;
        }
    }
}
