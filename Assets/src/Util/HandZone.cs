using Curry.Events;
using Curry.Explore;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
    public delegate void OnEncounter(IReadOnlyList<Encounter> encounters);
    public class HandZone : CardDropZone
    {
        [SerializeField] SelectionManager m_selectionManager = default;
        [SerializeField] TargetGuideHandler m_targetingGuide = default;
        [SerializeField] Transform m_cardHolderRoot = default;
        [SerializeField] CurryGameEventListener m_onCardDraw = default;
        [SerializeField] CurryGameEventListener m_onDiscardHand = default;

        protected Hand m_cardsInHand = new Hand();
        public event OnEncounter OnEncounterTrigger;
        protected void Start()
        {
            m_onCardDraw?.Init();
            m_onDiscardHand?.Init();
            // get starting hand
            DraggableCard[] cards = m_cardHolderRoot.GetComponentsInChildren<DraggableCard>();
            foreach (DraggableCard card in cards)
            {
                PrepareCard(card);
                m_cardsInHand.Add(card.Card);
            }
        }
        public void OnCardDrawn(EventInfo info)
        {
            if (info == null) return;
            if (info is CardDrawInfo draw)
            {
                OnEncounterTrigger?.Invoke(draw.Encounters);
                m_cardsInHand.AddRange(draw.CardsDrawn);

                foreach (AdventCard card in draw.CardsDrawn)
                {
                    PrepareCard(card.GetComponent<DraggableCard>());
                }
            }
        }
        public void PlayCard(AdventCard card, AdventurerStats stats)
        {
            OnCardPlayed(card.GetComponent<DraggableCard>());
            m_cardsInHand.PlayCard(card, stats);
        }
        public void DiscardHand()
        {
            m_cardsInHand.DiscardCards();
        }
        protected override void PrepareCard(DraggableCard draggable)
        {
            if (draggable == null)
            {
                return;
            }
            draggable.OnDragBegin += TargetGuide;
        }
        protected virtual void TargetGuide(DraggableCard draggable)
        {
            if (draggable is TileTargetCard targetingCard)
            {
                m_targetingGuide?.BeginLine(targetingCard.transform);
            }
        }
        protected virtual void OnCardPlayed(DraggableCard draggable)
        {
            draggable.OnDragBegin -= TargetGuide;
            m_targetingGuide?.Clear();
        }

        public delegate void OnDiscard(List<AdventCard> discarded);
        public class Hand
        {
            public IReadOnlyList<AdventCard> CardsInHand { get { return m_hand; } }

            List<AdventCard> m_hand = new List<AdventCard>();

            public void AddRange(IReadOnlyList<AdventCard> cards)
            {
                m_hand.AddRange(cards);
            }
            public void Add(AdventCard card)
            {
                m_hand.Add(card);
            }
            public void PlayCard(AdventCard card, AdventurerStats stats)
            {
                if (m_hand.Remove(card))
                {
                    card.CardEffect?.Invoke(stats);
                }

            }
            // Remove all cards in hand that doesn't retain
            public void DiscardCards()
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
    }
}