using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading;
using System.Xml.Linq;
using UnityEditor.Rendering.LookDev;

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
        public void Setup(CardAttribute attribute) 
        {
            SetCardName(attribute.Name);
            SetCardText(attribute.Description);
            SetCost(attribute.TimeCost.ToString());
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
        public void SetCost(string cost)
        {
            m_cost.text = cost;
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
