using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Events
{
    // Contains events that trigger from internal systems and listened to UI/player scripts
    // Idea inspired by Unite 2017 talk by Ryan Hipple
    // and improvements done by u/NeoDragonCP.
    [CreateAssetMenu(fileName = "GameEventSource", menuName = "Game Events/Create a game event for things to listen.", order = 1)]
    public class CurryGameEventSource : ScriptableObject
    {
        readonly HashSet<CurryGameEventListener> m_eventListeners = new HashSet<CurryGameEventListener>();
        HashSet<CurryGameEventListener> m_toRemove = new HashSet<CurryGameEventListener>();
        public void Listen(CurryGameEventListener callme)
        {
            m_eventListeners.Add(callme);
        }

        public void Unlisten(CurryGameEventListener unCallme)
        {
            if (m_eventListeners.Contains(unCallme))
            {
                m_toRemove.Add(unCallme);
            }
        }

        public void Broadcast(EventInfo eventInfo)
        {
            foreach (CurryGameEventListener listener in m_eventListeners)
            {
                listener.OnEventTriggered(eventInfo);
            }

            if (m_toRemove.Count > 0) 
            {
                foreach(CurryGameEventListener remove in m_toRemove) 
                {
                    m_eventListeners.Remove(remove);
                }
                m_toRemove.Clear();
            }
        }
    }
}


