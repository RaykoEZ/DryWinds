using System;
using UnityEngine;
namespace Curry.Events
{
    [Serializable]
    public class RescueObjective : GameObjective
    {
        [SerializeField] AmountAchieved m_numberRescue = default;
        [SerializeField] CurryGameEventListener m_onRescued = default;
        [SerializeField] CurryGameEventListener m_onOutOfTime = default;
        public override string Title { get { return "VIP Rescue"; } }

        public override string Description { get { return "Rescue VIP within time limit: " + m_numberRescue.Description; } }

        public override ICondition<IComparable> ObjectiveCondition { get { return m_numberRescue as ICondition<IComparable>; } }

        public override void Init()
        {
            m_onRescued?.Init();
            m_onOutOfTime?.Init();
        }

        public override void Shutdown()
        {
            m_onRescued?.Shutdown();
            m_onOutOfTime?.Shutdown();
        }

        public virtual void OnRescue(EventInfo info) 
        {
            Debug.Log("Rescued VIP in time, well done");
            if (ObjectiveCondition.UpdateProgress(1))
            {
                OnCompleteCallback();
            }
        }
        public virtual void OnOutOfTime(EventInfo info)
        {
            Debug.Log("Oof, you failed to rescue VIP in time.");
            OnFailCallback();
        }
    }
}
