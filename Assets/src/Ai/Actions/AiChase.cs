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
            float mod = args.Self.Emotion.Current.Hatred - args.Self.Emotion.Current.Fear;
            return mod * m_basePriority;
        }

        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count > 0 && args.Self.Emotion.Current.EmotionState != AiEmotionState.Fear;
        }

        public override void OnEnter(AiActionInput param)
        {
            Debug.Log("Chase");
            Transform target = ChooseTarget(param.WorldState.Enemies).transform;
            param.Controller.MoveTo(target);
        }

        protected override IEnumerator ExecuteInternal(AiActionInput param)
        {
            throw new NotImplementedException();
        }
    }
}
