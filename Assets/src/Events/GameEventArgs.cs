using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{
    public class InteractableArgs : EventArgs
    {
        Interactable m_interactable;
        public Interactable Interactable { get { return m_interactable; } }

        public InteractableArgs(Interactable interactable)
        {
            m_interactable = interactable;
        }
    }

    public class FloraArgs : EventArgs
    {
        Flora m_flora;
        public Flora Flora { get { return m_flora; } }

        public FloraArgs(Flora flora)
        {
            m_flora = flora;
        }
    }

    public class ItemArgs : EventArgs
    {
        BaseItem m_item;
        public BaseItem Item { get { return m_item; } }

        public ItemArgs(BaseItem item)
        {
            m_item = item;
        }
    }

    public class PlayerArgs : EventArgs
    {
        Player m_player;
        public Player Player { get { return m_player; } }

        public PlayerArgs(Player player)
        {
            m_player = player;
        }
    }

    public class NPCArgs : EventArgs
    {
        BaseNpc m_npc;
        public BaseNpc Npc { get { return m_npc; } }

        public NPCArgs(BaseNpc npc)
        {
            m_npc = npc;
        }
    }
}
