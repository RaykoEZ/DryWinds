using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.UI
{
    // A ui element for character ability 
    public class AbilityDisplay : MonoBehaviour
    {
        [SerializeField] PanelUIHandler m_uiHandler = default;
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] Image m_icon = default;
        [SerializeField] Image m_rangePattern = default;
        public void Setup(AbilityContent content) 
        {
            m_name.text = content.Name;
            m_description.text = content.Description;
            m_icon.sprite = content.Icon;
            m_rangePattern.sprite = content.RangePattern;
        }
        public void ResetDisplay() 
        {
            m_name.text = "";
            m_description.text = "";
            m_icon.sprite = null;
            m_rangePattern.sprite = null;
        }
        public void Show() 
        {
            m_uiHandler?.Show();
        }
        public void Hide() 
        {
            m_uiHandler?.Hide();
        }
    }
}