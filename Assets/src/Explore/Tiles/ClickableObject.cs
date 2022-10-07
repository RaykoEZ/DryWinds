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

        public TileSelectionInfo(
            TileSelectionMode mode,
            GameObject selected,
            Vector3 clickedPos,
            Dictionary<string, object> payload = null,
            Action onFinishCallback = null): base(payload, onFinishCallback) 
        {
            SelectionMode = mode;
            SelectedObject = selected;
            ClickScreenPosition = clickedPos;
        }
    }

    public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
    {
        [Serializable]
        [Flags]
        protected enum InputButtonFlag
        {
            Left = 1 << PointerEventData.InputButton.Left,
            Right = 1 << PointerEventData.InputButton.Right,
            Middle = 1 << PointerEventData.InputButton.Middle
        }

        [SerializeField] protected CurryGameEventTrigger m_onPointerClick = default;
        [SerializeField] protected TileSelectionMode m_selectionMode = default;
        [SerializeField] protected InputButtonFlag RegisterPointerClick = default;
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            // Checking if the registered button is pressed
            InputButtonFlag buttonEnum = (InputButtonFlag)(1 << (int)eventData.button);
            if ((RegisterPointerClick & buttonEnum) != 0) 
            {
                TileSelectionInfo info = new TileSelectionInfo(
                    m_selectionMode,
                    eventData.pointerEnter,
                    eventData.pressPosition);
                m_onPointerClick?.TriggerEvent(info);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}