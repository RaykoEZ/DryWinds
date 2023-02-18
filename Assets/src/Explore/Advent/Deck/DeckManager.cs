using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using System;
using Curry.Util;
using Curry.UI;

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
        [SerializeField] ChoicePrompter m_prompter = default;

        protected Inventory m_inventory;
        void Awake()
        {
            m_inventory = new Inventory();
            m_adventDb.Init(OnAdventLoadFinish);
            AddToInventory(m_startingInventory);
        }

        public void ChooseToAddFromInventory(ChoiceConditions conditions, Predicate<AdventCard> cardPoolFilter = null, Action onChosen = null) 
        {
            IReadOnlyList<AdventCard> cardPool = m_inventory.FilterInventory(cardPoolFilter);
            List<IChoice> choices = ChoiceUtil.ChooseCards_FromInstance(cardPool);
            OnChoiceFinish onChosenCallback = (result) => 
            {
                OnCardChosen(result);
                onChosen?.Invoke();
            };
            m_prompter.MakeChoice(conditions, choices, onChosenCallback);
        }
        void OnCardChosen(ChoiceResult result) 
        {
            if (result.Status == ChoiceResult.ChoiceStatus.Cancelled)
            {
                return;
            }
            List<AdventCard> cards = new List<AdventCard>();
            foreach (IChoice choice in result.Choices)
            {
                if (choice is CardChoice cardChoice && 
                    cardChoice.Value is AdventCard toTake)
                {
                    cardChoice.DisplayChoice(m_hand.transform);
                    CardChoice.DetachFromCard(cardChoice);
                    cards.Add(toTake);
                }
            }
            cards = m_inventory.TakeCards(cards);
            m_hand.AddCardsToHand(cards);
        }

        // randomly add cards from inventory to hand, filter method for limiting which type of cards to get/ignore 
        public void AddRandomFromInventory(int numToGet, Predicate<AdventCard> filter = null)
        {
            IReadOnlyList<AdventCard> cardPool = m_inventory.FilterInventory(filter);
            List<int> randomIndex = GameUtil.RandomRangeUnique(0, cardPool.Count, numToGet);
            List<AdventCard> cardsToAdd = new List<AdventCard>();
            AdventCard toTake;
            foreach (int i in randomIndex) 
            {
                toTake = m_inventory.TakeCard(cardPool[i]);
                cardsToAdd.Add(toTake);
            }
            m_hand.AddCardsToHand(cardsToAdd);
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
