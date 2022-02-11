using System;
using UnityEngine;
using Curry.Game;
using System.Collections;

namespace Curry.Ai
{
    [Serializable]
    [AddComponentMenu("Curry/Ai Action/Flee")]
    public class AiFlee : AiAction<IActionInput>
    {
        public override bool PreCondition(AiWorldState args)
        {
            // Only flee when scared and not already fleeing
            //Debug.Log($"Flee: {args.EmotionState}, {args.MovementState}");
            return args.EmotionState == AiEmotionState.Fear && args.MovementState != PathState.Fleeing;
        }

        protected override void ExecuteAction(AiActionInput param)
        {
            param.Controller.Flee();         
        }
    }
}
