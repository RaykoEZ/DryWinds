using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Collection
{
    public class HeldInventory
    {
        public static int MaxItemCount { get { return 5; } }
        public IReadOnlyList<ICollectable> Items { get { return m_items; } }
        protected List<ICollectable> m_items = new List<ICollectable>(MaxItemCount);

        public virtual ICollectable GetItem(int index) 
        {
            int i = Mathf.Clamp(index, 0, MaxItemCount);
            return m_items[i];
        }

        public virtual bool Add(ICollectable item) 
        {
            bool hasSpace = m_items.Count < MaxItemCount;
            if (hasSpace) 
            {
                m_items.Add(item);              
            }
            return hasSpace;
        }

        public virtual bool Remove(ICollectable item) 
        {
            return m_items.Remove(item);
        }
    }
}
