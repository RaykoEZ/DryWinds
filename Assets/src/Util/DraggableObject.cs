using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Util
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        Vector2 m_anchorOffset;
        Transform m_returnTo;
        public void Drop(Transform parent) 
        {
            m_returnTo = parent;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            m_returnTo = transform.parent;
            transform.SetParent(transform.parent.parent);
            Vector2 objectPos = eventData.pressEventCamera.WorldToScreenPoint(transform.position);
            m_anchorOffset = eventData.position - objectPos;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDragPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(m_returnTo);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        void SetDragPosition(PointerEventData e)
        {
            Vector2 worldPos = e.pressEventCamera.ScreenToWorldPoint(e.position - m_anchorOffset);
            transform.position = worldPos;
        }
    }
}