using Curry.Explore;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Util
{
    // onCancel: action to invoke when card activation is cancelled
    public delegate void OnCardDrop(AdventCard card, Action onDrop, Action onCancel);
    // For deploying any interactable from hand to play zone 
    public class CardDropZone : MonoBehaviour, IDropHandler
    {
        public event OnCardDrop OnDropped;
        // Called before the dropped card invokes its OnDragEnd,
        // trigger drop event when drag finishes (drop starts)
        public virtual void OnDrop(PointerEventData eventData)
        {
            DraggableCard draggable;
            if (eventData.pointerDrag.TryGetComponent(out draggable) && draggable.Droppable)
            {
                draggable.OnDragFinish += PrepareDrop;
            }
        }
        // Drop event, called after the draggable card finishes its OnDragEnd
        protected virtual void PrepareDrop(DraggableCard draggable)
        {
            draggable.OnDragFinish -= PrepareDrop;
            Action drop = () => { DropCard(draggable); };
            OnDropped?.Invoke(draggable.Card, drop, draggable.OnCancel);
        }
        void DropCard(DraggableCard draggable) 
        {
            int dropIdx = GetDropPosition(draggable.transform.position.x);
            draggable?.DropObject(transform, dropIdx);
        }
        // Called when card is dropped into this zone
        protected int GetDropPosition(float dropX)
        {
            int ret;
            for (ret = 0; ret < transform.childCount; ++ret)
            {
                if (dropX < transform.GetChild(ret).transform.position.x)
                {
                    break;
                }
            }
            return ret;
        }
    }
}