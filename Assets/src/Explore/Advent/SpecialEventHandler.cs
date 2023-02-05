using Curry.Events;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    // These events contain event cards to draw once only (until replenished)
    public class SpecialEventHandler : MonoBehaviour
    {
        [SerializeField] protected int m_specialEncounterId = default;
        [SerializeField] protected CurryGameEventTrigger m_onEncounter = default;
        public bool CanTrigger { get; set; } = true;
        public virtual void TriggerEvent() 
        {
            Debug.Log("IMPLEMENT SPECIAL EVENT");
            m_onEncounter?.TriggerEvent(new EncounterInfo(m_specialEncounterId));
            CanTrigger = false;
        }       
    }

}
