using Curry.Events;
using Curry.UI;
using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class TriggerProvision : PropertyAttribute
    {
        [SerializeField] CurryGameEventTrigger m_provisionEvent = default;
        [SerializeField] ChoiceConditions m_provisionConditions = default;
        public void ApplyEffect()
        {
            m_provisionEvent?.TriggerEvent(new ProvisionInfo(m_provisionConditions));
        }
    }
}