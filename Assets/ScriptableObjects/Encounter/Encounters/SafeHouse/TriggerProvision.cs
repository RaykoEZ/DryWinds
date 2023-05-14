using Curry.Events;
using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class TriggerProvision : PropertyAttribute
    {
        [SerializeField] CurryGameEventTrigger m_provisionEvent = default;
        public void ApplyEffect()
        {
            m_provisionEvent?.TriggerEvent(new EventInfo());
        }
    }
}