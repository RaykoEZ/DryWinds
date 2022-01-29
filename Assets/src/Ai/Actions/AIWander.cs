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
            return args.Enemies.Count == 0 || args.Self.Emotion.Current.EmotionState == AiEmotionState.Normal;
        }

        protected override IEnumerator ExecuteInternal(AiActionInput param)
        {
            while (PreCondition(param.WorldState)) 
            {
                float wait = UnityEngine.Random.Range(0.5f * m_cooldownTime, m_cooldownTime);
                yield return new WaitForSeconds(wait);
                Debug.Log("W!!!");
                param.Controller.Wander();
                yield return new WaitUntil(() => { return param.Controller.PathHandlerReachedTarget; });
            }
        }
    }
}
