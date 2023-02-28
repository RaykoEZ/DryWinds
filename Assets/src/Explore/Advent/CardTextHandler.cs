using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    public class CardTextHandler : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_nameField = default;
        public void SetCardText(string description) 
        {
            m_nameField.text = description;
        } 
        public void SetCardName(string name) 
        {
            m_nameField.text = name;
        }
    }
}
