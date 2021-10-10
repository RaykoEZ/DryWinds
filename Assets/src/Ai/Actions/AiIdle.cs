using System;
using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using Curry.Skill;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Idle", order = 0)]
    public class AiIdle : AiAction<IActionInput>
    {
        public override ICharacterAction<IActionInput> Execute(NpcController controller, NpcWorldState state)
        {
            return null;
        }
    }
}
