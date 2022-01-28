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
            return args.Self.Territories.Count > 0 && args.Self.Emotion.Current.EmotionState == AiEmotionState.Fear;
        }

        public override float Priority(AiWorldState args)
        {
            float mod = args.Self.Emotion.Current.Fear;
            return mod + m_basePriority;
        }

        protected override void ExecuteInternal(AiActionInput param)
        {
            Debug.Log("Fleeing!!!");
            NpcTerritory target = param.WorldState.Self.RandomRetreatLocation();
            if(target != null) 
            {
                param.Controller.InterruptAction();
                param.Controller.MoveTo(target.transform.position);
            }
        }
    }
}
