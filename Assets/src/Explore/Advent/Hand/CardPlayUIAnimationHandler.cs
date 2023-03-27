using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Explore
{
    // When pointer is in range, show ui, hide if not
    public class CardPlayUIAnimationHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] HideableUITrigger m_hidingUI = default;
        public void OnPointerEnter(PointerEventData eventData)
        {
            m_hidingUI.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_hidingUI.Hide();
        }
    }
}