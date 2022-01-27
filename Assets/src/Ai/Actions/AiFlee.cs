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
            return args.Enemies.Count > 0 && args.Emotion.EmotionState == AiEmotionState.Fear;
        }

        public override float Priority(AiWorldState args)
        {
            float mod = args.Emotion.Fear - (0.5f * args.Emotion.Hatred);
            return mod * m_basePriority;
        }

        protected override void ExecuteInternal(AiActionInput param)
        {
            Debug.Log("Fleeing!!!");
            param.Controller.Flee();
        }
    }
}
