using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{

    [Serializable]
    public class AddCardToHand : PropertyAttribute 
    {
        [SerializeField] List<AdventCard> m_cardsToAdd = default;
        public virtual void ApplyEffect(DeckManager deck)
        {
            deck.AddToHand(m_cardsToAdd);
        }
        public static void ApplyEffect(GameStateContext c, List<AdventCard> toAdd)
        {
            c.Deck.AddToHand(toAdd);
        }
    }
}
