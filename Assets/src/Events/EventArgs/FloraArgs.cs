using System;
using Curry.Game;

namespace Curry.Events
{
    public class FloraArgs : EventArgs
    {
        Flora m_flora;
        public Flora Flora { get { return m_flora; } }

        public FloraArgs(Flora flora)
        {
            m_flora = flora;
        }
    }
}
