using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{
    public class GameEventHandler : MonoBehaviour
    {
        [SerializeField] CurryGameEventListener m_onExitScreen = default;
        [SerializeField] CurryGameEventListener m_onKnockout = default;

        public event EventHandler<NPCEventArgs> OnNpcOffscreen;
        public event EventHandler<NPCEventArgs> OnNpcKnockout;

        public event EventHandler<PlayerEventArgs> OnPlayerOffscreen;
        public event EventHandler<PlayerEventArgs> OnPlayerKnockout;

        void Start() 
        {
            m_onExitScreen.Init();
            m_onKnockout.Init();
        }

        public void OnObjectExitScreen(EventInfo info) 
        {
            if (info.Payload["exitingObject"] is Interactable obj) 
            {
                if (obj is Player player)
                {
                    PlayerEventArgs args = new PlayerEventArgs(player);
                    OnPlayerOffscreen?.Invoke(this, args);

                }
                else if (obj is BaseNpc npc)
                {
                    NPCEventArgs args = new NPCEventArgs(npc);
                    OnNpcOffscreen?.Invoke(this, args);
                }
            }
        }

        public void OnObjectKnockout(EventInfo info)
        {
            if (info.Payload["exitingObject"] is Interactable obj)
            {
                if (obj is Player player) 
                {
                    PlayerEventArgs args = new PlayerEventArgs(player);
                    OnPlayerKnockout?.Invoke(this, args);

                }
                else if (obj is BaseNpc npc)
                {
                    NPCEventArgs args = new NPCEventArgs(npc);
                    OnNpcKnockout?.Invoke(this, args);
                }
            }
        }
    }
}
