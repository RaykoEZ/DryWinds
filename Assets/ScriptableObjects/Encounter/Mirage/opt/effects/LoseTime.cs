using Curry.Events;
using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class LoseTime : PropertyAttribute
    {
        [SerializeField] int m_timeLoss = default;
        [SerializeField] CurryGameEventTrigger m_onLoseTime = default;
        public void ApplyEffect()
        {
            TimeInfo info = new TimeInfo(m_timeLoss);
            m_onLoseTime?.TriggerEvent(info);
        }
    }
}