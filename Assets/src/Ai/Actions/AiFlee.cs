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
            bool ret = base.PreCondition(args) && 
                args.MovementState != PathState.Fleeing;
            // Only flee when scared and not already fleeing
            //Debug.Log($"Flee: {args.EmotionState}, {args.MovementState}");
            return ret;
        }

        protected override void ExecuteAction(AiActionInput param)
        {
            param.Controller.Flee();         
        }
    }
}
