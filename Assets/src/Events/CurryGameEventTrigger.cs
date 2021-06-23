using System;

namespace Curry.Events
{
    [Serializable]
    public class CurryGameEventTrigger
    {
        public CurryGameEventSource m_eventToTrigger = default;

        public void TriggerEvent(EventInfo eventInfo)
        {
            m_eventToTrigger?.Broadcast(eventInfo);
        }
    }

}


