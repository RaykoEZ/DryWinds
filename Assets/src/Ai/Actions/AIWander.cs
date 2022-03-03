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
            //Debug.Log($"Wander: {args.Enemies.Count == 0}, {args.MovementState}");
            return args.Enemies.Count == 0 && args.MovementState == PathState.Idle;
        }

        protected override void ExecuteAction(AiActionInput param)
        {
            param.Controller.Wander();
        }
    }
}
