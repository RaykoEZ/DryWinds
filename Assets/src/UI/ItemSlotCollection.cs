using UnityEngine;
using Curry.Game;
using Curry.Collection;

namespace Curry.UI
{
    public class ItemSlotCollection : MonoBehaviour
    {
        [SerializeField] protected ItemSlot[] m_itemSlots = new ItemSlot[HeldInventory.MaxItemCount];

        public void LoadItemToSlot(int slot, EntityProperty prop)
        {
            if(slot < m_itemSlots.Length) 
            {
                m_itemSlots[slot].LoadValues(prop);
            }
        }

        public void OnItemUsed(int slot, bool expired) 
        {
            if (expired) 
            {
                m_itemSlots[slot].Unload();
            }
        }

        protected void UnloadSlot(int slot)
        {
            if (slot < m_itemSlots.Length)
            {
                m_itemSlots[slot].Unload();
            }
        }
    }
}
