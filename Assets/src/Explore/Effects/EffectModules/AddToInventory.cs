using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class AddToInventory : PropertyAttribute
    {
        [SerializeField] List<CardAsset> m_cardsToAdd = default;
        public virtual void ApplyEffect(DeckManager deck)
        {
            deck.AddToInventory_FromAsset(m_cardsToAdd);
        }
    }
}