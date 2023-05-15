using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    public class LootManager : MonoBehaviour 
    {
        [SerializeField] DeckManager m_deck = default;

        public void ReceiveLoot(List<MonoBehaviour> items) 
        {
            List<AdventCard> cardsToAdd = new List<AdventCard>();
            foreach(var item in items) 
            { 
                // If item is a card, wee add it to the card list to add outside the loop
                if(item is AdventCard card) 
                {
                    cardsToAdd.Add(card);
                }
                // If we have other items of different types, catch them here in the future
            }
            m_deck.AddToHand(cardsToAdd);
        }
    }
}
