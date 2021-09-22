using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Idle", order = 0)]
    public class AiIdle : AiState
    {
        public override bool ExitCondition(NpcWorldState args)
        {
            throw new NotImplementedException();
        }
        public override bool PreCondition(NpcWorldState args)
        {
            throw new NotImplementedException();
        }

        public override void OnEnter(NpcController controller, NpcWorldState state)
        {

        }

        public override void OnUpdate(NpcController controller, NpcWorldState state)
        {
        }


    }
}
