using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "CardsInHand_", menuName = "Curry/GameState/Check for .../CardsInHand", order = 1)]
    public class CardsInHand : GameStateCondition
    {
        [SerializeField] List<CardAsset> m_needTheseInHand = default;
        protected static CompareCardByName m_compareByName = new CompareCardByName();
        public override bool IsConditionMet(GameStateContext context) 
        {
            bool ret = true;
            var hand = context.CardsInHand;
            DeckManager deck = context.Deck;
            List<AdventCard> searchFor = deck.InstantiateCards_FromAsset(m_needTheseInHand);
            foreach (var need in searchFor) 
            {                
                ret &= hand.Contains(need, m_compareByName);
            }
            return ret;
        }
    }
}