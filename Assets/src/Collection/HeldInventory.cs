using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Collection
{
    public class HeldInventory
    {
        public static int MaxItemCount { get { return 5; } }
        public IReadOnlyList<ICollectable> Items { get { return m_items; } }
        protected ICollectable[] m_items = new ICollectable[MaxItemCount];

        public virtual ICollectable GetItem(int index)
        {
            int i = Mathf.Clamp(index, 0, MaxItemCount - 1);
            return m_items[i];
        }

        public virtual bool Add(ICollectable item, out int slot) 
        {
            bool hasSpace = false;
            for (int i = 0; i < MaxItemCount; ++i) 
            {
                hasSpace = m_items[i] == null;
                if (hasSpace)
                {
                    m_items[i] = item;
                    slot = i;
                    return hasSpace;
                }
            }
            slot = -1;
            return hasSpace;
        }

        public void UseItem(int slot, out bool expired) 
        {
            ICollectable item = GetItem(slot);
            if (item == null)
            {
                expired = false;
                return;
            }
            expired = item.Activate();
            if (expired)
            {
                DiscardAt(slot);
            }
        }

        public virtual bool DiscardAt(int slot) 
        {
            bool itemExists = m_items != null;
            if (itemExists) 
            {
                m_items[slot].Discard();
                m_items[slot] = null;
            }
            return itemExists;
        }
    }
}
