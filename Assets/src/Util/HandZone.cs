using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Events;
using Curry.Explore;
namespace Curry.Util
{
    public delegate void OnEncounter(IReadOnlyList<Encounter> encounters);
    public class HandZone : CardDropZone
    {
        [SerializeField] Transform m_cardHolderRoot = default;
        [SerializeField] CurryGameEventListener m_onCardDraw = default;
        [SerializeField] CurryGameEventListener m_onDiscardHand = default;
        [SerializeField] Image m_playPanel = default;
        [SerializeField] SelectionManager m_selection = default;
        [SerializeField] CurryGameEventListener m_onDropTileSelected = default;
        [SerializeField] EndTurnTrigger m_endTurn = default;
        IPlayer m_playerRef;
        Transform m_playerTransform;
        // The card we are dragging into a play zone
        DraggableCard m_pendingCardRef;
        protected Hand m_cardsInHand = new Hand();
        // When a card, that targets a position, finishes targeting...
        public event OnCardDrop OnCardTargetResolve;
        internal void Init(IPlayer player, Transform playerTransform)
        {
            m_playerRef = player;
            m_playerTransform = playerTransform;
        }
        protected void Start()
        {
            m_onDropTileSelected?.Init();
            m_onCardDraw?.Init();
            m_onDiscardHand?.Init();
            m_endTurn.OnTurnEnd += DiscardHand;
            // get starting hand
            DraggableCard[] cards = m_cardHolderRoot.GetComponentsInChildren<DraggableCard>();
            foreach (DraggableCard card in cards)
            {
                PrepareCard(card);
                m_cardsInHand.Add(card.Card);
            }
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
            OnCardTargetResolve?.Invoke(m_pendingCardRef.Card, onPlay: null, onCancel: m_pendingCardRef.OnCancel);
        }
        internal void OnCardDrawn(EventInfo info)
        {
            if (info == null) return;
            if (info is CardDrawInfo draw)
            {
                HandleEncounters(draw.Encounters);
                m_cardsInHand.AddRange(draw.CardsDrawn);
                foreach (AdventCard card in draw.CardsDrawn)
                {
                    PrepareCard(card.GetComponent<DraggableCard>());
                }
            }
        }
        internal void PlayCard(AdventCard card, AdventurerStats stats)
        {
            // Reset pending card
            m_pendingCardRef = null;
            OnCardLeavesHand(card.GetComponent<DraggableCard>());
            HidePlayZone();
            m_cardsInHand.PlayCard(card, m_playerRef);
        }
        internal void DiscardHand()
        {
            foreach(AdventCard c in m_cardsInHand.CardsInHand) 
            {
                OnCardLeavesHand(c.GetComponent<DraggableCard>());
            }
            HidePlayZone();
            m_cardsInHand.DiscardCards();
        }
        internal void ShowPlayZone()
        {
            if (m_pendingCardRef != null && m_pendingCardRef.DoesCardNeedTarget)
            {
                ITargetsPosition targetCard = m_pendingCardRef.Card as ITargetsPosition;
                m_selection.SelectDropZoneTile(
                    targetCard.Range,
                    m_playerTransform);
            }
            else
            {
                m_playPanel.enabled = true;
            }
        }
        internal void HidePlayZone()
        {
            m_selection.CancelSelection();
            m_playPanel.enabled = false;
        }
        protected virtual void HandleEncounters(IReadOnlyList<Encounter> draw) 
        {
            foreach (Encounter encounter in draw)
            {
                encounter?.CardEffect?.Invoke(m_playerRef);
            }
        }
        protected override void PrepareCard(DraggableCard draggable)
        {
            if (draggable == null)
            {
                return;
            }
            draggable.OnReturn += OnCardReturn;
            draggable.OnDragBegin += TargetGuide;
        }
        void OnCardReturn(DraggableCard card)
        {
            m_pendingCardRef = null;
            HidePlayZone();
        }
        protected virtual void TargetGuide(DraggableCard draggable)
        {
            if (draggable.DoesCardNeedTarget)
            {
                m_pendingCardRef = draggable;
                m_selection?.TargetGuide(m_pendingCardRef.transform);
            }
            ShowPlayZone();
        }
        protected virtual void OnCardLeavesHand(DraggableCard draggable)
        {
            draggable.OnDragBegin -= TargetGuide;
            draggable.OnReturn -= OnCardReturn;
        }
        #region Hand Class
        public delegate void OnDiscard(List<AdventCard> discarded);
        public class Hand
        {
            public IReadOnlyList<AdventCard> CardsInHand { get { return m_hand; } }

            List<AdventCard> m_hand = new List<AdventCard>();

            internal void AddRange(IReadOnlyList<AdventCard> cards)
            {
                m_hand.AddRange(cards);
            }
            internal void Add(AdventCard card)
            {
                m_hand.Add(card);
            }
            internal void PlayCard(AdventCard card, IPlayer player)
            {
                if (m_hand.Remove(card))
                {
                    card.CardEffect?.Invoke(player);
                }

            }
            // Remove all cards in hand that doesn't retain
            internal void DiscardCards()
            {
                List<AdventCard> toDiscard = new List<AdventCard>();
                foreach (AdventCard card in m_hand)
                {
                    if (!card.RetainCard)
                    {
                        toDiscard.Add(card);
                    }
                }
                foreach (AdventCard card in toDiscard)
                {
                    m_hand.Remove(card);
                    card.OnDiscard();
                }
            }
        }
        #endregion
    }
}