using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Curry.Explore
{
    public class CardContentSetter : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_nameField = default;
        [SerializeField] TextMeshProUGUI m_descriptionField = default;
        [SerializeField] TextMeshProUGUI m_cost = default;
        [SerializeField] TextMeshProUGUI m_cooldown = default;
        [SerializeField] TextMeshProUGUI m_holdingValue = default;
        [SerializeField] GameObject m_consumableIcon = default;
        [SerializeField] List<Image> m_actionCostIcons = default;
        public void Setup(CardAttribute attribute) 
        {
            SetCardName(attribute.Name);
            SetCardText(attribute.Description);
            SetActionCost(attribute.Cost.ActionCount);
            SetTimeCost(attribute.Cost.Time.ToString());
            SetHoldingValue(attribute.HoldingValue);
        }
        public void SetCardText(string description) 
        {
            m_descriptionField.text = description;
        } 
        public void SetCardName(string name) 
        {
            m_nameField.text = name;
        }
        public void SetTimeCost(string cost)
        {
            m_cost.text = cost;
        }
        public void SetActionCost(int cost) 
        {
            int i = 0;
            foreach(var icon in m_actionCostIcons) 
            {
                icon.gameObject.SetActive(i < cost);
                ++i;
            }
        }
        public void SetCooldown(string cd)
        {
            m_cooldown.text = cd;
        }
        public void SetConsumableIcon(bool isConsumable = true)
        {
            m_consumableIcon.SetActive(isConsumable);
        }
        public void SetHoldingValue(int val) 
        {
            m_holdingValue.text = val.ToString();
        }
    }
}
