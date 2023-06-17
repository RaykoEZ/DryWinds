using Curry.Game;
using Curry.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public class Hand
    {
        protected int m_maxCapacity = 0;
        protected int m_totalHoldingValueInHand = 0;
        public int MaxCapacity { get => m_maxCapacity; protected set { m_maxCapacity = Mathf.Max(0, value); } }
        public int TotalHandHoldingValue => m_totalHoldingValueInHand;
        // Does player have higher card holding value than hand max capacity?
        public bool IsHandOverloaded => m_totalHoldingValueInHand > m_maxCapacity;
        public IReadOnlyList<DraggableCard> CardsInHand { get { return m_cardsInHand; } }

        List<DraggableCard> m_cardsInHand = new List<DraggableCard>();
        internal Hand(int maxCapacity) 
        {
            MaxCapacity = Mathf.Max(0, maxCapacity);
        }
        public bool ContainsCard(AdventCard card) 
        {
            return m_cardsInHand.Contains(card.GetComponent<DraggableCard>());
        }
        public void TakeCards(List<AdventCard> cards) 
        { 
        
        }
        internal void SetMaxCapacity(int newCapacity) 
        {
            MaxCapacity = newCapacity;
        }
        internal void AddCards(IReadOnlyList<DraggableCard> cards)
        {
            m_cardsInHand.AddRange(cards);
            foreach(var card in cards) 
            {
                card.GetComponent<CardInteractionController>()?.SetInteractionMode(
                    CardInteractMode.Play | CardInteractMode.Inspect);
                m_totalHoldingValueInHand += card.Card.HoldingValue;
            }
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
                m_totalHoldingValueInHand -= card.Card.HoldingValue;
            }
        }        
    }
}