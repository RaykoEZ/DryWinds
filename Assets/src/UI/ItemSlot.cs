using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Curry.Game;

namespace Curry.UI
{
    [Serializable]
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] Image m_image = default;
        [SerializeField] Sprite m_defaultIcon = default;
        public void LoadValues(
            EntityProperty slotData, ICollectable item)
        {
            m_image.sprite = slotData.EntitySprite;
            StoreItem(item.CollectedObject);
        }

        public void Unload()
        {
            m_image.sprite = m_defaultIcon;
        }

        protected void StoreItem(GameObject go) 
        {
            go.transform.SetParent(transform);
            go.transform.position = Vector3.zero;
            go.SetActive(false);
        }
    }
}
