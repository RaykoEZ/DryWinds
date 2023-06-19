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
        List<AdventCard> m_cardsInHand = new List<AdventCard>();
        CardDragHandler m_dragRef;
        public int MaxCapacity { get => m_maxCapacity; protected set { m_maxCapacity = Mathf.Max(0, value); } }
        public int TotalHandHoldingValue => m_totalHoldingValueInHand;
        // Does player have higher card holding value than hand max capacity?
        public bool IsHandOverloaded => m_totalHoldingValueInHand > m_maxCapacity;
        public IReadOnlyList<AdventCard> CardsInHand { get { return m_cardsInHand; } }
        internal Hand(int maxCapacity, CardDragHandler drag) 
        {
            MaxCapacity = Mathf.Max(0, maxCapacity);
            m_dragRef = drag;
        }
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
                OnCardLeaveHand(card.GetComponent<DraggableCard>());
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
                card.GetComponent<CardInteractionController>()?.SetInteractionMode(
                    CardInteractMode.Play | CardInteractMode.Inspect);
                PrepareCard(card);
                m_totalHoldingValueInHand += card.HoldingValue;
            }
        }
        internal virtual void PrepareCard(AdventCard card)
        {
            if (card == null)
            {
                return;
            }
            card.GetComponent<DraggableCard>().OnReturn += m_dragRef.OnCardReturn;
            card.GetComponent<DraggableCard>().OnDragBegin += m_dragRef.TargetGuide;
        }
        internal virtual void OnCardLeaveHand(DraggableCard drag) 
        {
            if (drag == null) return;
            drag.OnDragBegin -= m_dragRef.TargetGuide;
            drag.OnReturn -= m_dragRef.OnCardReturn;
        }
        internal void EnablePlay() 
        { 
            foreach(AdventCard card in CardsInHand) 
            {
                card.GetComponent<CardInteractionController>()?.SetInteractionMode(
                    CardInteractMode.Play | CardInteractMode.Inspect);
            }
        }
        internal void DisablePlay() 
        {
            foreach (AdventCard card in CardsInHand)
            {
                card.GetComponent<CardInteractionController>()?.
                    SetInteractionMode(CardInteractMode.Inspect);
            }
        }
        internal IEnumerator PlayCard(AdventCard card, IPlayer player)
        {
            if (m_cardsInHand.Contains(card))
            {
                yield return card.StartCoroutine(card.ActivateEffect(player));
            }
        }        
    }
}