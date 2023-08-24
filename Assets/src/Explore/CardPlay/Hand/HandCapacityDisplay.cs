using System.Collections;
using TMPro;
using UnityEngine;

namespace Curry.UI
{
    public class HandCapacityDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_capacityField = default;
        public void UpdateDisplay(int maxCapacity, int holdingValue) 
        {
            m_capacityField.text = holdingValue > maxCapacity?
                $" <color=red>{holdingValue} / {maxCapacity}</color>" :
                $"{holdingValue} / {maxCapacity}";
        }
    }
}