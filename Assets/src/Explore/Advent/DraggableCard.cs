using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Curry.Util;

namespace Curry.Explore
{
    // A class to trigger card effects when dragged and dropped
    public class DraggableCard : DraggableObject 
    {
        [SerializeField] AdventCard m_card = default;
        [SerializeField] UnityEvent m_onDragEffect = default;
        [SerializeField] UnityEvent m_onDropEffect = default;
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
            m_onDragEffect?.Invoke();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (Droppable) 
            {
                m_onDropEffect?.Invoke();
            }
        }
    }

}