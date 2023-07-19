using Curry.Events;
using Curry.Util;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Curry.Explore
{

    // A class to trigger card effects when dragged and dropped
    public class DraggableCard : DraggableObject
    {
        [Serializable]
        protected struct UITriggers
        {
            [SerializeField] CurryGameEventTrigger m_cardDragTrigger;
            [SerializeField] CurryGameEventTrigger m_cardDropTrigger;
            public CurryGameEventTrigger DragTrigger { get { return m_cardDragTrigger; } }
            public CurryGameEventTrigger DropTrigger { get { return m_cardDropTrigger; } }
        }
        [SerializeField] AdventCard m_card = default;
        [SerializeField] protected UITriggers m_ui = default;
        public delegate void OnCardDragUpdate(DraggableCard card);
        public event OnCardDragUpdate OnDragFinish;
        public event OnCardDragUpdate OnDragBegin;
        public event OnCardDragUpdate OnReturn;
        public AdventCard Card { get { return m_card; } }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (Draggable) 
            {
                base.OnBeginDrag(eventData);
                EventInfo info = new EventInfo();
                m_ui.DragTrigger?.TriggerEvent(info);
                OnDragBegin?.Invoke(this);
            }
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            // Let target validation activate card effect
            if (Droppable && Card.Resource is not ITargetsPosition)
            {
                EventInfo info = new EventInfo();
                m_ui.DropTrigger?.TriggerEvent(info);
            }
            base.OnEndDrag(eventData);
            OnDragFinish?.Invoke(this);
        }
        public virtual void OnCancel()
        {
            ReturnToBeforeDrag();
        }
        public override void ReturnToBeforeDrag()
        {
            base.ReturnToBeforeDrag();
            OnReturn?.Invoke(this);
        }
    }
}