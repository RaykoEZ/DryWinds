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
            throw new NotImplementedException();
        }

        public override void OnUpdate(NpcController controller)
        {
            throw new NotImplementedException();
        }
    }
}
