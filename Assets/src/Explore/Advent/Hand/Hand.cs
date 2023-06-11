using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnDiscard(List<AdventCard> discarded);
    public partial class HandManager
    {
        public class Hand
        {
            protected int m_maxCapacity = 0;
            protected int m_totalHoldingValueInHand = 0;
            public int MaxCapacity => m_maxCapacity;
            public int TotalHoldingValueInHand => m_totalHoldingValueInHand;
            // Does player have higher card holding value than hand max capacity?
            public bool IsHandOverloaded => m_totalHoldingValueInHand > m_maxCapacity;
            public IReadOnlyList<DraggableCard> CardsInHand { get { return m_cardsInHand; } }

            List<DraggableCard> m_cardsInHand = new List<DraggableCard>();
            internal Hand(int maxCapacity) 
            {
                m_maxCapacity = Mathf.Max(0, maxCapacity);
            }
            internal void AddRange(IReadOnlyList<DraggableCard> cards)
            {
                m_cardsInHand.AddRange(cards);
                foreach(var card in cards) 
                {
                    m_totalHoldingValueInHand += card.Card.HoldingValaue;
                }
            }
            internal void Add(DraggableCard card)
            {
                m_cardsInHand.Add(card);
                m_totalHoldingValueInHand += card.Card.HoldingValaue;
            }
            internal void EnablePlay() 
            { 
                foreach(DraggableCard card in CardsInHand) 
                {
                    card.enabled = true;
                }
            }
            internal void DisablePlay() 
            {
                foreach (DraggableCard card in CardsInHand)
                {
                    card.enabled = false;
                }
            }
            internal IEnumerator PlayCard(DraggableCard card, IPlayer player)
            {
                if (m_cardsInHand.Remove(card))
                {
                    yield return card.StartCoroutine(card.Card.ActivateEffect(player));
                    m_totalHoldingValueInHand -= card.Card.HoldingValaue;
                }
            }
        }
    }
}