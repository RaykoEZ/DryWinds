using Curry.Explore;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Curry.UI
{
    // A UI item to display a bespoke character modifier
    public class ModifierIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] PanelUIHandler m_uiHandler = default;
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] TextMeshProUGUI m_expire = default;
        [SerializeField] Image m_icon = default;
        bool m_ready = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        void Show() 
        {
            if (m_ready) 
            {
                m_uiHandler?.Show();
            }
        }
        void Hide() 
        {
            m_uiHandler?.Hide();      
        }
        public void SetupIcon(ModifierContent content) 
        {
            m_name.text = content.Name;
            m_description.text = content.Description;
            m_icon.sprite = content.Icon;
            m_icon.color = Color.white;
            m_ready = true;
        }
        public void ResetIcon()
        {
            Hide();
            m_ready = false;
            m_name.text = "";
            m_description.text = "";
            m_expire.text = "";
            m_icon.sprite = null;
            m_icon.color = Color.clear;
        }
    }
}