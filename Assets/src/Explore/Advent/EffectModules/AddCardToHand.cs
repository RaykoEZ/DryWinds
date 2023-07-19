using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class AddCardToHand : PropertyAttribute 
    {
        [SerializeField] List<CardAsset> m_cardsToAdd = default;
        public virtual void ApplyEffect(DeckManager deck)
        {
            deck.AddToHand_FromAsset(m_cardsToAdd);
        }
    }
}
