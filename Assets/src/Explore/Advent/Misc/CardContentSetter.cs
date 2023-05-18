using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Curry.Explore
{
    [Serializable]
    public struct CardTypeSpriteSetting 
    {
        public Sprite Skill;
        public Sprite Equipment;
        public Sprite Item;
    }
    public class CardContentSetter : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_nameField = default;
        [SerializeField] TextMeshProUGUI m_descriptionField = default;
        [SerializeField] TextMeshProUGUI m_cost = default;
        [SerializeField] TextMeshProUGUI m_cooldown = default;
        [SerializeField] CardTypeSpriteSetting m_cardTypeSprites = default;
        [SerializeField] Image m_cardTypeIcon = default;
        [SerializeField] GameObject m_consumableIcon = default;
        public void SetCardText(string description) 
        {
            m_descriptionField.text = description;
        } 
        public void SetCardName(string name) 
        {
            m_nameField.text = name;
        }
        public void SetCost(string cost)
        {
            m_cost.text = cost;
        }
        public void SetCooldown(string cd)
        {
            m_cooldown.text = cd;
        }
        public void SetCardType(CardType type) 
        {
            m_cardTypeIcon.enabled = true;
            switch (type)
            {
                case CardType.Skill:
                    m_cardTypeIcon.sprite = m_cardTypeSprites.Skill;
                    break;
                case CardType.Equipment:
                    m_cardTypeIcon.sprite = m_cardTypeSprites.Equipment;
                    break;
                case CardType.Item:
                    m_cardTypeIcon.sprite = m_cardTypeSprites.Item;
                    break;
                default:
                    m_cardTypeIcon.enabled = false;
                    break;
            }
        }
        public void SetConsumableIcon(bool isConsumable = true)
        {
            m_consumableIcon.SetActive(isConsumable);
        }
    }
}
