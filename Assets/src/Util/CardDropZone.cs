using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Curry.Explore;

namespace Curry.Util
{
    public delegate void OnCardDrop(DraggableCard card);
    // For deploying any interactable from hand to play zone 
    public class CardDropZone : MonoBehaviour, IDropHandler
    {
        public OnCardDrop OnDropped;

        public void OnDrop(PointerEventData eventData)
        {
            DraggableCard draggable;
            if(eventData.pointerDrag.TryGetComponent(out draggable) && draggable.Droppable) 
            {
                int dropIdx = GetDropPosition(draggable.transform.position.x);
                draggable?.Drop(transform, dropIdx);
                OnDropped?.Invoke(draggable);
            }
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