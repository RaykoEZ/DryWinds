using System;
using Curry.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Hand
    {
        [SerializeField] protected int m_maxCapacity = default;
        protected int m_totalHoldingValueInHand = 0;
        List<AdventCard> m_cardsInHand = new List<AdventCard>();
        public int MaxCapacity { get => m_maxCapacity; protected set { m_maxCapacity = Mathf.Max(0, value); } }
        public int TotalHandHoldingValue => m_totalHoldingValueInHand;
        // Does player have higher card holding value than hand max capacity?
        public bool IsHandOverloaded => m_totalHoldingValueInHand > m_maxCapacity;
        public IReadOnlyList<AdventCard> CardsInHand { get { return m_cardsInHand; } }
        public List<AdventCard> TakeCards(List<AdventCard> cards) 
        {
            List<AdventCard> ret = new List<AdventCard>();
            foreach(AdventCard card in cards) 
            {
                if (TakeCard(card)) 
                {
                    ret.Add(card);
                }
            }
            return ret;
        }
        public bool TakeCard(AdventCard card) 
        {
            if (m_cardsInHand.Remove(card))
            {
                m_totalHoldingValueInHand -= card.HoldingValue;                       
                return true;
            }
            else
            {
                return false;
            }
        }
        internal void AddCards(IReadOnlyList<AdventCard> cards)
        {
            m_cardsInHand.AddRange(cards);
            foreach(var card in cards) 
            {
                m_totalHoldingValueInHand += card.HoldingValue;
            }
        }
        internal IEnumerator PlayCard(GameStateContext context, AdventCard card, IPlayer player)
        {
            if (m_cardsInHand.Contains(card))
            {
                yield return card.StartCoroutine(card.ActivateEffect(player, context));
            }
        }        
    }
}