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
        [Serializable]
        protected struct FXTriggers
        {
            [SerializeField] UnityEvent m_cardDragTrigger;
            [SerializeField] UnityEvent m_cardDropTrigger;
            public UnityEvent DragTrigger { get { return m_cardDragTrigger; } }
            public UnityEvent DropTrigger { get { return m_cardDropTrigger; } }
        }

        [SerializeField] AdventCard m_card = default;
        [SerializeField] protected UITriggers m_ui = default;
        [SerializeField] protected FXTriggers m_fx = default;

        public delegate void OnCardDragUpdate(DraggableCard card);
        public event OnCardDragUpdate OnDragFinish;
        public event OnCardDragUpdate OnDragBegin;
        public event OnCardDragUpdate OnReturn;

        public override bool Droppable
        {
            get
            {
                return m_card.Activatable;
            }
        }
        public bool DoesCardNeedTarget { get { return Card is ITargetsPosition; } }
        public bool Draggable { get; set; } = true;
        protected override Transform OnDragParent
        {
            get { return DoesCardNeedTarget ? transform.parent : base.OnDragParent; }
        }
        public AdventCard Card { get { return m_card; } }
        
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (Draggable) 
            {
                base.OnBeginDrag(eventData);
                EventInfo info = new EventInfo();
                m_ui.DragTrigger?.TriggerEvent(info);
                m_fx.DragTrigger?.Invoke();
                OnDragBegin?.Invoke(this);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (Droppable)
            {
                EventInfo info = new EventInfo();
                m_ui.DropTrigger?.TriggerEvent(info);
                m_fx.DropTrigger?.Invoke();
            }
            base.OnEndDrag(eventData);
            OnDragFinish?.Invoke(this);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            // Do not move when drag is held, if the card needs to use a target guide
            if (!DoesCardNeedTarget) 
            {
                base.OnDrag(eventData);
            }
        }
        public virtual void OnCancel()
        {
            ReturnToBeforeDrag();
        }

        protected override void ReturnToBeforeDrag()
        {
            base.ReturnToBeforeDrag();
            OnReturn?.Invoke(this);
        }
    }

}