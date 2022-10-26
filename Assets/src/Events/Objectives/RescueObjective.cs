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

        public override string Description { get { return m_numberRescue.Description; } }

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
            if (m_numberRescue.UpdateProgress(1))
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
