using System;
using UnityEngine;
using Curry.Explore;

namespace Curry.Events
{
    [Serializable]
    public class RescueObjective : GameObjective
    {
        [SerializeField] AmountAchieved m_numberRescue = default;
        public override string Title { get { return "VIP Rescue"; } }
        public override string Description { get { return m_numberRescue.Description; } }

        public virtual void OnRescue()
        {
            Debug.Log("Rescued VIP in time, well done");
            if (m_numberRescue.UpdateProgress(1))
            {
                OnCompleteCallback();
            }
        }
        public virtual void OnFailure()
        {
            OnFailCallback();
        }
    }
}
