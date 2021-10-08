using System;
using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using Curry.Skill;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Idle", order = 0)]
    public class AiIdle : AiAction<IActionInput, SkillProperty>
    {
        public override ICharacterAction<IActionInput, SkillProperty> Execute(NpcController controller, NpcWorldState state)
        {
            return null;
        }

        protected override float ActionScore(ICharacterAction<IActionInput, SkillProperty> action, BaseCharacter target)
        {
            throw new NotImplementedException();
        }

        public  override ICharacterAction<IActionInput, SkillProperty> ChooseAction(
            List<ICharacterAction<IActionInput, SkillProperty>> skills, BaseCharacter target)
        {
            throw new NotImplementedException();
        }
    }
}
