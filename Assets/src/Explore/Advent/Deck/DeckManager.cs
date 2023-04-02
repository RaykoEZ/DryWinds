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
        [SerializeField] protected Inventory m_inventory = default;
        [SerializeField] protected CardDatabase m_adventDb = default;
        [SerializeField] AdventInstanceManager m_instance = default;
        [SerializeField] HandManager m_hand = default;
        [SerializeField] List<AdventCard> m_startingInventory = default;
        [SerializeField] ChoicePrompter m_prompter = default;

        void Start()
        {
            m_adventDb.Init(OnAdventLoadFinish);
        }
        // Choosing cards to add to hand, from inventory.
        public void ChooseToAddFromInventory(ChoiceConditions conditions, Predicate<AdventCard> cardPoolFilter = null, Action onChosen = null) 
        {
            IReadOnlyList<AdventCard> cardPool = m_inventory.FilterInventory(cardPoolFilter);
            List<IChoice> choices = CloneCardChoice(cardPool as List<AdventCard>);
            OnChoiceFinish onChosenCallback = (result) => 
            {
                OnCardChosen(result);
                onChosen?.Invoke();
            };
            m_prompter.MakeChoice(conditions, choices, onChosenCallback);
        }
        // Instantiate a clone of cards for selection and inspection
        List<IChoice> CloneCardChoice(List<AdventCard> cardSource) 
        {
            List<AdventCard> copies = new List<AdventCard>();
            // Instantiate copies
            // and assgn their Value property to the real card in inventory for later process
            foreach(AdventCard card in cardSource) 
            {
                AdventCard copy = InstantiateCard(card);
                copy.GetComponent<CardInteractionController>()?.
                    Init(card, CardInteractMode.Inspect | CardInteractMode.Select);
                copies.Add(copy);
            }
            List<IChoice> ret = ChoiceUtil.ChooseCards(copies);
            return ret;
        }
        // When player chose cards to add...
        void OnCardChosen(ChoiceResult result) 
        {
            if (result.Status == ChoiceResult.ChoiceStatus.Confirmed)
            {
                // Process all chosen cards before the choice components are destroyed 
                List<AdventCard> cards = new List<AdventCard>();
                foreach (IChoice choice in result.Chosen)
                {
                    if (choice.Value is AdventCard toTake)
                    {
                        CardInteractionController sourceCardController = 
                            toTake.GetComponent<CardInteractionController>();
                        sourceCardController?.DisplayChoice(m_hand.transform);
                        cards.Add(toTake);
                    }
                }
                cards = m_inventory.TakeCards(cards);
                m_hand.AddCardsToHand(cards);
            }      
            // Remove copies
            foreach (IChoice choice in result.ChoseFrom)
            {
                if (choice is CardInteractionController cardChoice)
                {
                    cardChoice.transform.SetParent(m_instance.DefaultParent);
                    cardChoice.GetComponent<AdventCard>()?.ReturnToPool();
                }
            }
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
        public void MoveToInventory(AdventCard move) 
        {
            m_inventory.Add(move);
        }
        // Instantiate cards and trigger game events OnCardDraw
        public void AddToHand(List<AdventCard> cardsToDraw)
        { 
            List<AdventCard> cardInstances = InstantiateCards(cardsToDraw);
            m_hand.AddCardsToHand(cardInstances);
        }
        List<AdventCard> InstantiateCards(List<AdventCard> refs, CardInteractMode interactMode = CardInteractMode.Inspect) 
        {
            List<AdventCard> ret = new List<AdventCard>();
            foreach (AdventCard cardRef in refs)
            {
                // Instantiating cards to be drawn
                AdventCard cardInstance = InstantiateCard(cardRef, interactMode);
                ret.Add(cardInstance);
            }
            return ret;
        }
        AdventCard InstantiateCard(AdventCard cardRef, CardInteractMode interactMode = CardInteractMode.Inspect)
        {
            AdventCard ret;
            ret = m_instance.GetInstanceFromAsset(cardRef.gameObject);
            ret.GetComponent<CardInteractionController>()?.Init(ret, interactMode);
            return ret;
        }
        void OnAdventLoadFinish()
        {
            foreach (KeyValuePair<int, AdventCard> advent in m_adventDb.AdventList)
            {
                m_instance.PrepareNewInstance(advent.Value.gameObject);
            }
            AddToInventory(m_startingInventory);
        }
    }

}
