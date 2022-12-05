using UnityEngine;
using UnityEngine.EventSystems;
using Curry.Events;

namespace Curry.Explore
{
    // A card that needs to drag and select a tile/position to activate.
    public class TileTargetCard : DraggableCard
    {
        [SerializeField] CardTargetingHandler m_line = default;
        [SerializeField] CurryGameEventTrigger m_onCardTargetTile = default;
        // Make line and change cursor icon to target a tile
        public override void OnBeginDrag(PointerEventData eventData)
        {
            m_line?.BeginLine(transform.position);
            base.OnBeginDrag(eventData);
            EventInfo info = new EventInfo();
        }
        // Move line render towards cursor position
        public override void OnDrag(PointerEventData eventData)
        {
            m_line?.UpdateLine(eventData);
        }
        // Check if there is a valid target selected, cancel card activation if not
        public override void OnEndDrag(PointerEventData eventData)
        {
            m_line?.Clear();
            base.OnEndDrag(eventData);
        }
    }

}