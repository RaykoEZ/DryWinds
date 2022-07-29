using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Util
{
    public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        Vector2 m_anchorOffset;
        Transform m_returnTo;
        public void OnBeginDrag(PointerEventData eventData)
        {
            m_returnTo = transform.parent;
            transform.SetParent(transform.parent.parent);
            Vector2 objectPos = eventData.pressEventCamera.WorldToScreenPoint(transform.position);
            m_anchorOffset = eventData.position - objectPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDragPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(m_returnTo);
        }

        void SetDragPosition(PointerEventData e)
        {
            Vector2 worldPos = e.pressEventCamera.ScreenToWorldPoint(e.position - m_anchorOffset);
            transform.position = worldPos;
        }
    }
}