using UnityEngine;
using UnityEngine.EventSystems;
using Curry.Explore;

namespace Curry.Util
{
    // For deploying any interactable from hand to play zone 
    public class CardDropZone : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            DraggableObject draggable;
            if(eventData.pointerDrag.TryGetComponent(out draggable)) 
            {
                draggable?.Drop(transform);
            }
        }
    }
}