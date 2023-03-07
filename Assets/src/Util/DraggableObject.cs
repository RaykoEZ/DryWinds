using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Util
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        Vector2 m_anchorOffset;
        Transform m_origin;
        int m_originIndex;
        public virtual bool Droppable { get { return true; } }
        // Move one above original parent when dragging the object 
        protected virtual Transform OnDragParent => transform.parent.parent;
        void OnEnable() 
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        protected virtual void SetDropPosition(Transform parent, int siblingIndex = 0) 
        {
            m_origin = parent;
            m_originIndex = siblingIndex;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            LeaveOrigin(eventData);
        }
        protected virtual void LeaveOrigin(PointerEventData eventData)
        {
            // Set default drop to return to hand
            SetDropPosition(transform.parent, transform.GetSiblingIndex());
            // move parent to intermediate parent until we see a drop zone/return to original parent 
            transform.SetParent(OnDragParent);
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
            ReturnToBeforeDrag();
        }
        public virtual void DropObject(Transform parent, int siblingIndex = 0)
        {
            transform.SetParent(parent, false);
            transform.SetSiblingIndex(siblingIndex);
        }
        public virtual void ReturnToBeforeDrag() 
        {
            DropObject(m_origin, m_originIndex);
        }
        void SetDragPosition(PointerEventData e)
        {
            Vector2 worldPos = e.pressEventCamera.ScreenToWorldPoint(e.position - m_anchorOffset);
            GetComponent<RectTransform>().position = worldPos;
        }
    }

}