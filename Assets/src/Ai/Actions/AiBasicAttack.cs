using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/BasicAttack", order = 0)]
    public class AiBasicAttack : AiSkill
    {
        public override float Priority(AiWorldState args)
        {
            float mod = args.Emotion.Hatred - (0.5f * args.Emotion.Fear);
            return mod * m_basePriority;
        }

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
