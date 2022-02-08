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
            var territories = args.Self.Territories;
            bool inTerritory = false;
            foreach(NpcTerritory territory in territories) 
            {
                if (territory.Boundary.Contains(transform.position)) 
                {
                    inTerritory = true;
                    break;
                }
            }
            return !inTerritory && args.Self.Territories.Count > 0 && args.Self.Emotion.Current.EmotionState == AiEmotionState.Fear;
        }

        public override float Priority(AiWorldState args)
        {
            float mod = args.Self.Emotion.Current.Fear;
            return mod + m_basePriority;
        }

        protected override void OnActionFinish()
        {
            m_fleeing = false;
            base.OnActionFinish();
        }

        protected override IEnumerator ExecuteInternal(AiActionInput param)
        {
            Debug.Log("Fleeing!!!");
            NpcTerritory target = param.WorldState.Self.RandomTerritory();
            if(target != null && !m_fleeing) 
            {
                m_fleeing = true;
                param.Controller.MoveTo(target.transform.position);
            }
            yield return new WaitUntil(()=> { return param.Controller.PathHandlerReachedTarget; });
            m_fleeing = false;
        }
    }
}
