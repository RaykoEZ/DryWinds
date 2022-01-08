using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Flee", order = 0)]
    public class AiFlee : AiAction<IActionInput>
    {
        public override ICharacterAction<IActionInput> Execute(NpcController controller, AiWorldState state)
        {
            return null;
        }
    }
}
