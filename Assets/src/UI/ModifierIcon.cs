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

        public void OnPointerEnter(PointerEventData eventData)
        {
            Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        public void Show() 
        {
            m_uiHandler?.Show();
        }
        public void Hide() 
        {
            m_uiHandler?.Hide();
        }
        public void SetupIcon(ModifierContent content) 
        {
            m_name.text = content.Name;
            m_description.text = content.Description;
            m_icon.sprite = content.Icon;
        }
        public void ResetIcon()
        {
            Hide();
            m_name.text = "";
            m_description.text = "";
            m_expire.text = "";
            m_icon.sprite = null;
        }
    }
}