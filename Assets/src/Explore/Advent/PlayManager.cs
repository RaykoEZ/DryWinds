using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Util;
using Curry.Events;

namespace Curry.Explore
{
    public class PlayManager : MonoBehaviour 
    {
        [SerializeField] TimeManager m_time = default;
        [SerializeField] CardDropZone m_playZone = default;
        [SerializeField] Adventurer m_player = default;
        [SerializeField] CurryGameEventListener m_onCardBeginDrag = default;
        [SerializeField] CurryGameEventListener m_onCardDropped = default;
        [SerializeField] CurryGameEventListener m_onCardDraw = default;
        [SerializeField] CurryGameEventListener m_onDiscardHand = default;
        [SerializeField] CurryGameEventTrigger m_cardActivated = default;
        [SerializeField] Image m_playPanel = default;
        Hand m_hand = new Hand();

        protected void Awake()
        {
            m_onCardDraw?.Init();
            m_onCardBeginDrag?.Init();
            m_onCardDropped?.Init();
            m_onDiscardHand?.Init();
        }
        void OnEnable()
        {
            m_time.OnOutOfTimeTrigger += OutOfTime;
            m_playZone.OnDropped += OnCardPlayed;
        }

        void OnDisable()
        {
            m_time.OnOutOfTimeTrigger -= OutOfTime;
            m_playZone.OnDropped -= OnCardPlayed;
        }

        void OutOfTime(int timeSpent) 
        {
            Debug.Log("Out of Time");
        }

        public void ShowPlayZone()
        {
            m_playPanel.enabled = true;
        }
        public void HidePlayZone()
        {
            m_playPanel.enabled = false;
        }

        void OnCardPlayed(DraggableCard card) 
        {
            // Activate & Spend Time/Resource
            m_time.TrySpendTime(card.Card.TimeCost, out bool enoughTime);
            if (enoughTime) 
            {
                PositionInfo info = new PositionInfo(m_player.Stats.WorldPosition);
                m_cardActivated?.TriggerEvent(info);
                m_hand.PlayCard(card.Card, m_player.Stats);
            }
            HidePlayZone();
        }

        public void OnCardDrawn(EventInfo info) 
        {
            if (info == null) return;
            if(info is CardDrawInfo draw) 
            {
                OnEncounterDraw(draw.Encounters);
                m_hand.Draw(draw.CardsDrawn);
            }
        }

        void OnEncounterDraw(IReadOnlyList<Encounter> draw) 
        {
            int totalCost = 0;
            foreach(Encounter encounter in draw) 
            {
                totalCost += encounter.TimeCost;
                encounter?.OnDrawEffect(m_player.Stats);
            }
            m_time.TrySpendTime(totalCost, out bool _);
        }

        public void DiscardHand() 
        {
            m_hand.DiscardCards();
        }


        public delegate void OnDiscard(List<AdventCard> discarded);
        public class Hand
        {
            public IReadOnlyList<AdventCard> CardsInHand { get { return m_hand; } }
            public event OnDiscard OnDiscardHand;

            List<AdventCard> m_hand = new List<AdventCard>();

            public void Draw(IReadOnlyList<AdventCard> cards)
            {
                m_hand.AddRange(cards);
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
                OnDiscardHand?.Invoke(toDiscard);
            }
        }

    }
}