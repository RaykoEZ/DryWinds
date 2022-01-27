using System;
using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using Curry.Skill;
using System.Collections;

namespace Curry.Ai
{
    [Serializable]
    [AddComponentMenu("Curry/Ai Action/Chase")]
    public class AiChase : AiAction<IActionInput>
    {
        public override float Priority(AiWorldState args)
        {
            float mod = args.Emotion.Hatred - args.Emotion.Fear;
            return mod * m_basePriority;
        }

        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count > 0 && args.Emotion.EmotionState != AiEmotionState.Fear;
        }

        public override void Execute(AiActionInput param)
        {
            Debug.Log("Chase");
            Transform target = ChooseTarget(param.WorldState.Enemies).transform;
            param.Controller.MoveTo(target);
        }

        protected override void ExecuteInternal(AiActionInput param)
        {
            throw new NotImplementedException();
        }
    }
}
