using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
using Curry.Events;

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
        public void Setup(Vector3 moveTo, AbilityContent content) 
        {
            m_name.text = content.Name;
            m_description.text = content.Description;
            m_icon.sprite = content.Icon;
            string name = $"Detail/Range/{content.Name}";
            // Display range preview on range Display
            m_rangeDisplay.ShowRange(
                name,
                m_rangePreview, 
                content.RangePattern);
            m_rangeDisplay.MoveRangeTileTo(name, m_rangePreview, moveTo);

        }
        public void ResetDisplay() 
        {
            m_name.text = "";
            m_description.text = "";
            m_icon.sprite = null;
            //Reset Range display
            m_rangeDisplay?.HidePrompt();
        }
    }
}