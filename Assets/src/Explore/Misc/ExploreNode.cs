using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Curry.Skill;

namespace Curry.Explore
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ExploreNode : MonoBehaviour, IPointerClickHandler
    {
        protected abstract void OnDiscovered();
        protected abstract void OnReached();
        protected abstract void OnLeave();

        protected virtual void OnTriggerEnter2D(Collider2D collider) 
        {
            PathDrone drone = collider.gameObject.GetComponent<PathDrone>();
            if (drone != null) 
            {
                OnReached();
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collider)
        {
            PathDrone drone = collider.gameObject.GetComponent<PathDrone>();
            if (drone != null)
            {
                OnLeave();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on location: " + name);
        }
    }

}