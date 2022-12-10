using UnityEngine;
using UnityEngine.EventSystems;
using Curry.Events;

namespace Curry.Explore
{
    // A card that needs to drag and select a tile/position to activate.
    public class TileTargetCard : DraggableCard
    {
        // We don't move parents when dragging for targeting
        protected override Transform OnDragParent => transform.parent;
        public override void OnDrag(PointerEventData eventData)
        {
            // Do not move when drag is held
        }
    }
}