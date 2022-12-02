using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Curry.Explore;
using Curry.Events;

namespace Curry.Util
{
    // onCancel: action to invoke when card activation is cancelled
    public delegate void OnCardDrop(AdventCard card, Action onPlay, Action onCancel);
    // For deploying any interactable from hand to play zone 
    public class CardDropZone : MonoBehaviour, IDropHandler
    {
        public OnCardDrop OnDropped;
        // Called before the dropped card invokes its OnDragEnd, defer drop event
        public void OnDrop(PointerEventData eventData)
        {
            DraggableCard draggable;
            if(eventData.pointerDrag.TryGetComponent(out draggable) && draggable.Droppable) 
            {
                draggable.OnDragFinish += DeferDropEvent;
            }
        }
        // Drop event, called after the draggable card finishes its OnDragEnd
        void DeferDropEvent(DraggableCard draggable) 
        {
            draggable.OnDragFinish -= DeferDropEvent;
            int dropIdx = GetDropPosition(draggable.transform.position.x);
            Action drop = () =>
            {
                draggable?.DropObject(transform, dropIdx);
            };
            OnDropped?.Invoke(draggable.Card, drop, draggable.OnCancel);
        }
        int GetDropPosition(float dropX) 
        {
            int ret;
            for( ret = 0; ret < transform.childCount; ++ret ) 
            { 
                if(dropX < transform.GetChild(ret).transform.position.x) 
                {
                    break;
                }    
            }
            return ret;
        }
    }
}