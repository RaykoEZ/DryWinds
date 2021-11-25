using System;
using Curry.Game;

namespace Curry.Events
{
    public class PlayerArgs : EventArgs
    {
        Player m_player;
        public Player Player { get { return m_player; } }

        public PlayerArgs(Player player)
        {
            m_player = player;
        }
    }
}
