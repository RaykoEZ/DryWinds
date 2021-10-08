using System;
using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using Curry.Skill;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Chase", order = 0)]
    public class AiChase : AiAction<IActionInput, IActionProperty>
    {
        public override ICharacterAction<IActionInput, IActionProperty> Execute(NpcController controller, NpcWorldState state)
        {
            throw new NotImplementedException();
        }

        protected override float ActionScore(ICharacterAction<IActionInput, IActionProperty> action, BaseCharacter target)
        {
            throw new NotImplementedException();
        }

        public override ICharacterAction<IActionInput, IActionProperty> ChooseAction(
            List<ICharacterAction<IActionInput, IActionProperty>> skills, BaseCharacter target)
        {
            throw new NotImplementedException();
        }
    }
}
