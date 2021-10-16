using System;
using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using Curry.Skill;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Chase", order = 0)]
    public class AiChase : AiAction<IActionInput>
    {
        public override bool PreCondition(NpcWorldState args)
        {
            return args.Enemies.Count > 0;
        }

        public override ICharacterAction<IActionInput> Execute(NpcController controller, NpcWorldState state)
        {
            Transform target = ChooseTarget(state.Enemies).transform;
            controller.Move(target);
            return null;
        }
    }
}
