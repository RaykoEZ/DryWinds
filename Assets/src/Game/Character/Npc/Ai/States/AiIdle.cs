using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Idle", order = 0)]
    public class AiIdle : AiState
    {
        public override void OnEnter(NpcController controller)
        {
        }

        public override void OnUpdate(NpcController controller)
        {
        }
    }
}
