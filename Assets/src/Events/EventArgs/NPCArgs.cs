using System;
using Curry.Game;

namespace Curry.Events
{
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
