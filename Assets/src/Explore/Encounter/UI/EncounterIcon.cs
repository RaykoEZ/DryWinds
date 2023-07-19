using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Explore
{
    public delegate void OnEncounterTrigger();
    public class EncounterIcon : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Animator m_anim = default;
        public event OnEncounterTrigger OnTrigger;
        public void Show() 
        {
            m_anim.SetBool("show", true);
        }
        public void Hide() 
        {
            m_anim.SetBool("show", false);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Hide();
            OnTrigger?.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
        }
        public void OnPointerDown(PointerEventData eventData)
        {
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
        }
        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
