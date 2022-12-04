using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Curry.Util;
using Curry.Events;

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
        [SerializeField] UITriggers m_ui = default;
        [SerializeField] FXTriggers m_fx = default;

        public delegate void OnCardDragUpdate(DraggableCard card);
        public event OnCardDragUpdate OnDragFinish;

        public override bool Droppable { 
            get 
            {
                return m_card.Activatable;
            }   
        }

        public AdventCard Card { get { return m_card; } }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            EventInfo info = new EventInfo();
            m_ui.DragTrigger?.TriggerEvent(info);
            m_fx.DragTrigger?.Invoke();
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

        public virtual void OnCancel() 
        {
            ReturnToBeforeDrag();
        }
    }

}