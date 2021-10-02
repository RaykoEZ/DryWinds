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
            return args.Enemies.Count > 0 && args.BasicSkills.Count > 0;
        }

        protected override BaseSkill ChooseSkill(List<BaseSkill> skills, TargetPosition target)
        {
            BaseSkill ret = skills[0];
            float highScore = 0f;
            if (skills.Count > 1)
            {
                for (int i = 1; i < skills.Count; ++i)
                {
                    float score = SkillScore(skills[i], target);
                    ret = score > highScore ? skills[i] : ret;
                }
            }
            return ret;
        }

        protected override float SkillScore(BaseSkill skill, TargetPosition target)
        {
            if (!skill.SkillUsable)
            {
                return 0f;
            }

            SkillProperty properties = skill.SkillProperties;
            // base damage score
            float ret = (properties.SkillValue - 0.1f * properties.MaxWindupTime) / properties.SpCost;
            return ret;
        }
    }
}
