using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Util
{
    public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        Vector2 m_anchorOffset;
        public void OnBeginDrag(PointerEventData eventData)
        {           
            Vector2 objectPos = eventData.enterEventCamera.WorldToScreenPoint(transform.position);
            m_anchorOffset = eventData.position - objectPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDragPosition(eventData);
        }
        void SetDragPosition(PointerEventData e)
        {
            Vector2 worldPos = e.pressEventCamera.ScreenToWorldPoint(e.position - m_anchorOffset);
            transform.localPosition = worldPos;
        }
    }
}