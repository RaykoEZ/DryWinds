using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
namespace Curry.UI
{
    // A ui element for character ability 
    public class AbilityDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] Image m_icon = default;
        public void Setup(AbilityContent content) 
        {
            m_name.text = content.Name;
            m_description.text = content.Description;
            m_icon.sprite = content.Icon;
            // Display range preview on range Display
        }
        public void ResetDisplay() 
        {
            m_name.text = "";
            m_description.text = "";
            m_icon.sprite = null;
            //Reset Range display
        }
    }
}