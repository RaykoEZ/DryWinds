using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Curry.Events;

namespace Curry.Explore
{
    [Serializable]
    public enum TileSelectionMode 
    { 
        Preview,
        Play
    }
    public class TileSelectionInfo : EventInfo
    {
        public TileSelectionMode SelectionMode { get; protected set; }
        public GameObject SelectedObject { get; protected set; }
        public Vector3 ClickScreenPosition { get; protected set; }
        public PointerEventData.InputButton Button { get; protected set; }

        public TileSelectionInfo(
            TileSelectionMode mode,
            GameObject selected,
            Vector3 clickedPos,
            PointerEventData.InputButton button,
            Dictionary<string, object> payload = null,
            Action onFinishCallback = null): base(payload, onFinishCallback) 
        {
            SelectionMode = mode;
            SelectedObject = selected;
            ClickScreenPosition = clickedPos;
            Button = button;
        }
    }

        public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] CurryGameEventTrigger m_onPointerClick = default;
        [SerializeField] TileSelectionMode m_selectionMode = default;
        public void OnPointerClick(PointerEventData eventData)
        {
            TileSelectionInfo info = new TileSelectionInfo(
                m_selectionMode,
                eventData.pointerEnter,
                eventData.pressPosition,
                eventData.button);
            m_onPointerClick?.TriggerEvent(info);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}