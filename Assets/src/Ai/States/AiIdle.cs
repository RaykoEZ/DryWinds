﻿using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
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
