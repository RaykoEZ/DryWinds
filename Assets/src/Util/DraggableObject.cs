using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Util
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        Vector2 m_anchorOffset;
        Transform m_returnTo;
        int m_returnIndex;
        public virtual bool Droppable { get { return true; } }
        public virtual void Drop(Transform parent, int siblingIndex = 0) 
        {
            m_returnTo = parent;
            m_returnIndex = siblingIndex;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            m_returnTo = transform.parent;
            transform.SetParent(transform.parent.parent);
            Vector2 objectPos = eventData.pressEventCamera.WorldToScreenPoint(transform.position);
            m_anchorOffset = eventData.position - objectPos;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            SetDragPosition(eventData);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(m_returnTo);
            transform.SetSiblingIndex(m_returnIndex);
        }

        void SetDragPosition(PointerEventData e)
        {
            Vector2 worldPos = e.pressEventCamera.ScreenToWorldPoint(e.position - m_anchorOffset);
            transform.position = worldPos;
        }
    }

}