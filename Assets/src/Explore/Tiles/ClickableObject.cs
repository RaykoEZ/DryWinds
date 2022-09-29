using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Curry.Events;

namespace Curry.Explore
{
    public class ClickableObject : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] CurryGameEventTrigger m_onPointerClick = default;
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 pos = eventData.pressPosition;
            Dictionary<string, object> payload = new Dictionary<string, object> { {"pressPosition", pos } };
            EventInfo info = new EventInfo(payload);
            m_onPointerClick?.TriggerEvent(info);
        }
    }
}