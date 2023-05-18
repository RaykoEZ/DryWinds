using JetBrains.Annotations;
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
    }

    [Serializable]
    public struct LootDrop : IWeightedItem
    {
        [SerializeField] int m_oddsWeight;
        [SerializeField] List<AdventCard> m_lootItems;
        public int Weight => m_oddsWeight;
        public List<AdventCard> LootItems => m_lootItems;
    }
}
