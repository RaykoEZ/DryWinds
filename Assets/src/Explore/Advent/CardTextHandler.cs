using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    public class CardTextHandler : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_descriptionField = default;
        public void SetCardText(string description) 
        {
            m_descriptionField.text = description;
        } 
    }
}
