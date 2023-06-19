using UnityEngine;
using Curry.Util;
using Curry.Events;

namespace Curry.Explore
{
    public class CardDragHandler : MonoBehaviour
    {
        [SerializeField] Transform m_playerTransform = default;
        [SerializeField] PlayZone m_playZone = default;
        [SerializeField] SelectionManager m_selection = default;
        [SerializeField] CurryGameEventListener m_onDropTileSelected = default;
        void Start()
        {
            m_onDropTileSelected?.Init();
        }
        // The card we are dragging into a play zone
        DraggableCard m_pendingCardRef;
        // When a card, that targets a position, finishes targeting...
        public event OnCardDrop OnCardTargetResolve;
        public void ResetDragTarget()
        {
            m_pendingCardRef = null;
        }
        public void OnTargetDropZoneSelected(EventInfo info)
        {
            if (m_pendingCardRef == null) return;

            if (info is PositionInfo pos)
            {
                // Activate card effect with target
                ITargetsPosition handler = m_pendingCardRef.Card as ITargetsPosition;
                handler.SetTarget(pos.WorldPosition);
            }
            // do activation validation
            OnCardTargetResolve?.Invoke(m_pendingCardRef.Card, onDrop: null, onCancel: m_pendingCardRef.OnCancel);
        }
        public void TargetGuide(DraggableCard draggable)
        {
            if (draggable.Card is ITargetsPosition)
            {
                m_pendingCardRef = draggable;
                m_selection?.TargetGuide(m_pendingCardRef.transform);
            }
            ShowDropZones();
        }
        public void OnCardReturn(DraggableCard card)
        {
            m_pendingCardRef = null;
            HideDropZone();
        }
        public void ShowDropZones() 
        {
            if (m_pendingCardRef != null && m_pendingCardRef.Card is ITargetsPosition target)
            {
                m_selection.SelectDropZoneTile(m_pendingCardRef.Card.Name, target.TargetingRange, m_playerTransform);
            }
            else 
            {
                m_playZone.SetPlayZonrActive(true);
            }
        }
        public void HideDropZone()
        {
            m_selection.CancelSelection();
            m_playZone.SetPlayZonrActive(false);
        }
    }
}