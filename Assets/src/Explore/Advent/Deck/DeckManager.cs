using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using System;

namespace Curry.Explore
{
    // Contained database for all available advent(cards and decks)
    // Quiries Tile info for a tile in its tilemap coordinate
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] protected CardDatabase m_adventDb = default;
        [SerializeField] AdventInstanceManager m_instance = default;
        [SerializeField] HandManager m_hand = default;
        [SerializeField] List<AdventCard> m_startingInventory = default;
        protected Inventory m_inventory;
        void Awake()
        {
            m_inventory = new Inventory();
            AddToInventory(m_startingInventory);
            m_adventDb.Init(OnAdventLoadFinish);
        }
        // randomly add cards from inventory to hand, filter method for limiting which type of cards to get/ignore 
        public void AddRandomFromInventory(int numToGet, Predicate<AdventCard> filter = null)
        {
        
        }
        public void AddToInventory(List<AdventCard> add)
        {
            List<AdventCard> cardInstances = InstantiateCards(add);
            m_inventory.AddRange(cardInstances);
        }
        // Instantiate cards and trigger game events OnCardDraw
        public void AddToHand(List<AdventCard> cardsToDraw)
        {
            List<AdventCard> cardInstances = InstantiateCards(cardsToDraw);
            m_hand.AddCardsToHand(cardInstances);
        }
        List<AdventCard> InstantiateCards(List<AdventCard> refs) 
        {
            List<AdventCard> ret = new List<AdventCard>();
            foreach (AdventCard cardRef in refs)
            {
                // Instantiating cards to be drawn
                AdventCard cardInstance = InstantiateCard(cardRef);
                ret.Add(cardInstance);
            }
            return ret;
        }
        AdventCard InstantiateCard(AdventCard cardRef)
        {
            AdventCard ret;
            ret = m_instance.GetInstanceFromAsset(cardRef.gameObject);
            return ret;
        }

        void OnAdventLoadFinish()
        {
            foreach (KeyValuePair<int, AdventCard> advent in m_adventDb.AdventList)
            {
                m_instance.PrepareNewInstance(advent.Value.gameObject);
            }
        }
    }

}
