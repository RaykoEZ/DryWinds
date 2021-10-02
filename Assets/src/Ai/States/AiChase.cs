using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Chase", order = 0)]
    public class AiChase : AiAction
    {
        public override void Execute(NpcController controller, NpcWorldState state)
        {
            throw new NotImplementedException();
        }
    }
}
