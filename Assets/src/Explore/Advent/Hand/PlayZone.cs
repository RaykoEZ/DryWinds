using Curry.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.Explore
{
    public delegate void OnCardPlayed(GameStateContext context, AdventCard played, Action onDrop, Action onCancel);
    public class PlayZone : CardDropZone 
    {
        [SerializeField] Image m_playPanel = default;
        GameStateContext m_contextRef;
        public event OnCardPlayed OnPlayed;
        public void Init(GameStateContext context)
        {
            m_contextRef = context;
        }
        public void SetPlayZoneActive(bool playable = true) 
        {
            m_playPanel.enabled = playable;
        }
        protected override void PrepareDrop(DraggableCard draggable)
        {
            base.PrepareDrop(draggable);
            OnPlayed?.Invoke(m_contextRef, draggable.Card, () => DropCard(draggable), draggable.OnCancel);
        }
    }
}