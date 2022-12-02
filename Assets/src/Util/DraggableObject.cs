﻿using System;
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
        protected virtual void SetDropPosition(Transform parent, int siblingIndex = 0) 
        {
            m_returnTo = parent;
            m_returnIndex = siblingIndex;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            // Set default drop to return to hand
            SetDropPosition(transform.parent, transform.GetSiblingIndex());
            // move parent one above
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
            ReturnToBeforeDrag();
        }
        public virtual void DropObject(Transform parent, int siblingIndex = 0)
        {
            transform.SetParent(parent);
            transform.SetSiblingIndex(siblingIndex);
        }
        protected void ReturnToBeforeDrag() 
        {
            DropObject(m_returnTo, m_returnIndex);
        }
        void SetDragPosition(PointerEventData e)
        {
            Vector2 worldPos = e.pressEventCamera.ScreenToWorldPoint(e.position - m_anchorOffset);
            transform.position = worldPos;
        }
    }

}