using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{
    public class InteractableEventArgs : EventArgs
    {
        Interactable m_interactable;
        public Interactable Interactable { get { return m_interactable; } }

        public InteractableEventArgs(Interactable interactable)
        {
            m_interactable = interactable;
        }
    }
    public class PlayerEventArgs : EventArgs
    {
        Player m_player;
        public Player Player { get { return m_player; } }

        public PlayerEventArgs(Player player)
        {
            m_player = player;
        }
    }

    public class NPCEventArgs : EventArgs
    {
        BaseNpc m_npc;
        public BaseNpc Interactable { get { return m_npc; } }

        public NPCEventArgs(BaseNpc npc)
        {
            m_npc = npc;
        }
    }
}
