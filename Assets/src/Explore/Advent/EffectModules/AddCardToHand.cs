using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    public interface ICardEconomyModule
    {
        void ApplyEffect(DeckManager deck);
    }
    [Serializable]
    public class AddCardToHand : PropertyAttribute, ICardEconomyModule 
    {
        [SerializeField] List<AdventCard> m_cardsToAdd = default;
        public virtual void ApplyEffect(DeckManager deck) 
        {
            deck.AddToHand(m_cardsToAdd);
        }
    }


}
