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
        bool m_fleeing = false;
        public override bool PreCondition(AiWorldState args)
        {
            return args.EmotionState == AiEmotionState.Fear;
        }

        protected override void OnActionFinish()
        {
            m_fleeing = false;
            Debug.Log("Flee Finish");
            base.OnActionFinish();
        }

        protected override IEnumerator ExecuteInternal(AiActionInput param)
        {
            Debug.Log("Flee");
            if(!m_fleeing) 
            {
                m_fleeing = true;
                param.Controller.Flee();
            }
            yield return new WaitUntil(()=> { return param.Controller.PathHandlerReachedTarget; });
        }
    }
}
