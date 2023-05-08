using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
using Curry.Events;
using System.Collections.Generic;

namespace Curry.UI
{
    // A ui element for character ability 
    public class AbilityDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] Image m_icon = default;
        [SerializeField] RangeDisplayHandler m_rangeDisplay = default;
        [SerializeField] GameObject m_rangePreview = default;
        List<string> m_abilityNamesInPreview = new List<string>();
        public void Setup(Vector3 offset, AbilityContent content) 
        {
            m_rangeDisplay.HidePrompt();
            m_name.text = content.Name;
            m_description.text = content.Description;
            m_icon.sprite = content.Icon;
            string name = $"Detail/Range/{content.Name}";
            // Display range preview on range Display
            m_rangeDisplay.ShowRange(
                name,
                m_rangePreview,
                content.TargetingRange);
            if (!m_abilityNamesInPreview.Contains(name)) 
            {
                m_rangeDisplay.MoveRangeTile(name, m_rangePreview, offset);
                m_abilityNamesInPreview.Add(name);
            }
        }
        public void ResetDisplay() 
        {
            foreach(string name in m_abilityNamesInPreview) 
            {
                m_rangeDisplay.ClearRangeTile(name, m_rangePreview);
            }
            m_abilityNamesInPreview.Clear();
            m_name.text = "";
            m_description.text = "";
            m_icon.sprite = null;
            //Reset Range display
            m_rangeDisplay?.HidePrompt();
        }
    }
}