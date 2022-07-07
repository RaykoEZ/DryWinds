using UnityEngine;
using System.Collections.Generic;
using Curry.Game;

namespace Curry.Events
{
    // Broadcast the object that exits the region collider
    [RequireComponent(typeof(Collider2D))]
    public class RegionExitTrigger : MonoBehaviour
    {
        [SerializeField] protected CurryGameEventTrigger m_trigger = default;
        [SerializeField] Collider2D m_region = default;

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            Interactable interactable = collision.gameObject.GetComponent<Interactable>();
            if (interactable != null && !m_region.bounds.Contains(interactable.transform.position)) 
            {
                Dictionary<string, object> args = new Dictionary<string, object>
                { {"exitingObject", interactable}};

                EventInfo info = new EventInfo(args);
                m_trigger.TriggerEvent(info);
            }
        }
    }
}
