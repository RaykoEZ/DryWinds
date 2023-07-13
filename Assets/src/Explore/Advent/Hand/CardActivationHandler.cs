using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    // Handles card activation triggers after a card finishes targeting a position
    public class CardActivationHandler : MonoBehaviour
    {
        [SerializeField] Transform m_playerTransform = default;
        [SerializeField] PlayZone m_playZone = default;
        [SerializeField] SelectionManager m_selection = default;
        [SerializeField] CurryGameEventListener m_onDropTileSelected = default;
        [SerializeField] ActionCostHandler m_cost = default;
        GameStateContext m_contextRef;
        public void Init(GameStateContext c)
        {
            m_contextRef = c;
            m_onDropTileSelected?.Init();
        }
        // The card we are dragging into a play zone
        DraggableCard m_pendingCardRef;
        // When a card, that targets a position, finishes targeting...
        public event OnCardPlayed OnCardTargetResolve;
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
            OnCardTargetResolve?.Invoke(m_contextRef, m_pendingCardRef.Card, onDrop: null, onCancel: m_pendingCardRef.OnCancel);
        }
        public void TargetGuide(DraggableCard draggable)
        {
            AdventCard card = draggable.Card;
            if (card is ITargetsPosition)
            {
                m_pendingCardRef = draggable;
                m_selection?.TargetGuide(m_pendingCardRef.transform);
            }
            m_cost?.BeginPreview(card.Cost);
            ShowDropZones();
        }
        public void OnCardReturn(DraggableCard _)
        {
            m_pendingCardRef = null;
            m_cost?.CancelPreview();
            HideDropZone();
        }
        public void ActivateHandEffect(IHandEffect handEffect) 
        {
            handEffect?.HandEffect(m_contextRef);
        }
        public void RevertHandEffect(IHandEffect handEffect) 
        {
            handEffect?.OnLeaveHand(m_contextRef);
        }
        public void ShowDropZones() 
        {
            if (m_pendingCardRef != null && m_pendingCardRef.Card is ITargetsPosition target)
            {
                m_selection.SelectDropZoneTile(m_pendingCardRef.Card.Name, target.TargetingRange, m_playerTransform);
            }
            else 
            {
                m_playZone.SetPlayZoneActive(true);
            }
        }
        public void HideDropZone()
        {
            m_selection.CancelSelection();
            m_playZone.SetPlayZoneActive(false);
        }
        public void EndOfTurnEffect(IEndOfTurnEffect card) 
        {
            card?.OnEndOfTurn(m_contextRef);
        }
    }
}