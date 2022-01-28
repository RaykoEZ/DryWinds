using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;
using System.Collections;

namespace Curry.Ai
{
    [Serializable]
    [AddComponentMenu("Curry/Ai Action/Wander")]
    public partial class AIWander : AiAction<IActionInput>
    {
        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count == 0;
        }

        protected override void ExecuteInternal(AiActionInput param)
        {
            Debug.Log("W!!!");
            param.Controller.Wander();
        }
    }
}
