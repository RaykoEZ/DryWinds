using System;
using UnityEngine.Events;

namespace Curry.Events
{
    [Serializable]
    public class CurryEvent : UnityEvent<EventInfo>
    { }

    [Serializable]
    public class CurryGameEventListener
    {
        public CurryGameEventSource m_eventToListen = default;
        public CurryEvent m_responses = new CurryEvent();

        public void Init()
        {
            m_eventToListen?.Listen(this);

        }

        public void Shutdown()
        {
            m_eventToListen?.Unlisten(this);

        }

        public void OnEventTriggered(EventInfo eventInfo)
        {
            m_responses?.Invoke(eventInfo);
        }
    }

}


