using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
                int dropIdx = GetDropPosition(draggable.transform.position.x);
                draggable?.Drop(transform, dropIdx);
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