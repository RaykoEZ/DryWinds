using System;
using Curry.Game;

namespace Curry.Events
{
    public class ItemArgs : EventArgs
    {
        BaseItem m_item;
        public BaseItem Item { get { return m_item; } }

        public ItemArgs(BaseItem item)
        {
            m_item = item;
        }
    }
}
