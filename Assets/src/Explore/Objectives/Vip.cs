using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    public interface IRescue
    {
        // When player interacts with vip, do this
        void Rescue();
    }
    public class Vip : MonoBehaviour, IRescue
    {
        // Happens when player finds clue/comms locations
        [SerializeField] CurryGameEventTrigger m_onRescued = default;

        public virtual void Rescue() 
        {
            Debug.Log("tyty");
            EventInfo info = new EventInfo();
            m_onRescued?.TriggerEvent(info);
        }
    }
}
