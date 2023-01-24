using System.Collections;
using System.Collections.Generic;

namespace Curry.Explore
{
    public delegate void OnDiscard(List<AdventCard> discarded);
    public partial class HandManager
    {
        public class Hand
        {
            public IReadOnlyList<DraggableCard> CardsInHand { get { return m_hand; } }

            List<DraggableCard> m_hand = new List<DraggableCard>();

            internal void AddRange(IReadOnlyList<DraggableCard> cards)
            {
                m_hand.AddRange(cards);
            }
            internal void Add(DraggableCard card)
            {
                m_hand.Add(card);
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
                if (m_hand.Remove(card))
                {
                    yield return card.StartCoroutine(card.Card.ActivateEffect(player));
                    card.Card.ReturnToPool();
                }
            }
            // Remove all cards in hand that doesn't retain
            internal void DiscardCards()
            {
                List<DraggableCard> toDiscard = new List<DraggableCard>();
                foreach (DraggableCard card in m_hand)
                {
                    toDiscard.Add(card);
                }
                foreach (DraggableCard card in toDiscard)
                {
                    m_hand.Remove(card);
                    card.Card.ReturnToPool();
                }
            }
        }
    }
}