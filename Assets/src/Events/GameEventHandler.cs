using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{
    public class GameEventHandler : MonoBehaviour
    {
        [SerializeField] CurryGameEventListener m_onKnockout = default;
        [SerializeField] CurryGameEventListener m_onItemObtain = default;
        [SerializeField] CurryGameEventListener m_onPlayerInteract = default;

        public event EventHandler<NPCArgs> OnNpcKnockout;
        public event EventHandler<PlayerArgs> OnPlayerKnockout;

        public event EventHandler<ItemArgs> OnItemObtained;
        public event EventHandler<FloraArgs> OnFloraObtained;

        public event EventHandler<InteractableArgs> OnInteract;
        public event EventHandler<NPCArgs> OnInteractNPC;

        void OnEnable() 
        {
            m_onKnockout.Init();
            m_onItemObtain.Init();
            m_onPlayerInteract.Init();
        }

        void OnDisable()
        {
            m_onKnockout.Shutdown();
            m_onItemObtain.Shutdown();
            m_onPlayerInteract.Shutdown();
        }

        public void OnObjectKnockout(EventInfo info)
        {
            if (info.Payload["exitingObject"] is Interactable obj)
            {
                if (obj is Player player) 
                {
                    PlayerArgs args = new PlayerArgs(player);
                    OnPlayerKnockout?.Invoke(this, args);

                }

                if (obj is BaseNpc npc)
                {
                    NPCArgs args = new NPCArgs(npc);
                    OnNpcKnockout?.Invoke(this, args);
                }
            }
        }

        public void OnItemObtain(EventInfo info)
        {
            if (info.Payload["collected"] is BaseItem item)
            {
                if (item is Flora flora)
                {

                }


            }
        }

        public void OnPlayerInteract(EventInfo info)
        {
            if (info.Payload["interactingWith"] is Interactable obj)
            {
                if (obj is BaseNpc npc)
                {

                }
            }
        }
    }
}
