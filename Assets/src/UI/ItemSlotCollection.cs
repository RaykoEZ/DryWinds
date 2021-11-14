using UnityEngine;
using Curry.Game;
using Curry.Collection;

namespace Curry.UI
{
    public class ItemSlotCollection : MonoBehaviour
    {
        [SerializeField] protected ItemSlot[] m_itemSlots = new ItemSlot[HeldInventory.MaxItemCount];

        public void LoadItemToSlot(int slot, EntityProperty prop, ICollectable obj)
        {
            if(slot < m_itemSlots.Length) 
            {
                m_itemSlots[slot].LoadValues(prop, obj);
            }
        }

        public void UnloadSlot(int slot)
        {
            if (slot < m_itemSlots.Length)
            {
                m_itemSlots[slot].Unload();
            }
        }
    }
}
