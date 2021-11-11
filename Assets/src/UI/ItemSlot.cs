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
        [SerializeField] Button m_skillIcon = default;
        [SerializeField] TextMeshProUGUI m_amount = default;
        [SerializeField] Image m_image = default;
        [SerializeField] Sprite m_defaultIcon = default;
        public void LoadValues(
            EntityProperty slotData)
        {
            m_image.sprite = slotData.ItemSprite;
        }

        public void Unload()
        {
            m_image.sprite = m_defaultIcon;
        }
    }
}
