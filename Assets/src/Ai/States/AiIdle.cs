using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Idle", order = 0)]
    public class AiIdle : AiAction
    {
        public override void Execute(NpcController controller, NpcWorldState state)
        {

        }
    }
}
