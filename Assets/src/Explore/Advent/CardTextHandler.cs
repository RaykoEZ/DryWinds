using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    public class CardTextHandler : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_nameField = default;
        [SerializeField] TextMeshProUGUI m_descriptionField = default;
        public void SetCardText(string description) 
        {
            m_descriptionField.text = description;
        } 
        public void SetCardName(string name) 
        {
            m_nameField.text = name;
        }
    }
}
