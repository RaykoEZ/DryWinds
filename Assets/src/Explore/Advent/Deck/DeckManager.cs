using System.Collections.Generic;
using UnityEngine;
using System;
using Curry.Util;
using Curry.UI;

namespace Curry.Explore
{
    // Contained database for all available advent(cards and decks)
    // Quiries Tile info for a tile in its tilemap coordinate
    public class DeckManager : SceneInterruptBehaviour
    {
        [SerializeField] protected Inventory m_inventory = default;
        [SerializeField] protected CardDatabase m_cardPool = default;
        [SerializeField] AdventInstanceManager m_instance = default;
        [SerializeField] HandManager m_hand = default;
        [SerializeField] AdventCard m_cardRef = default;
        [SerializeField] List<CardAsset> m_startingInventoryToLoad = default;
        [SerializeField] ChoicePrompter m_prompter = default;
        bool m_isReady = false;
        public bool IsReady { get => m_isReady; protected set => m_isReady = value; }
        void Awake()
        {
            m_cardPool.LoadAsset(OnAdventLoadFinish);
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
                EndInterrupt();
            };
            StartInterrupt();
            m_prompter.MakeChoice(conditions, choices, onChosenCallback);
        }
        // Instantiate a clone of cards for selection and inspection
        public List<IChoice> CloneCardChoice(List<AdventCard> cardSource)
        {
            List<AdventCard> copies = new List<AdventCard>();
            // Instantiate copies
            // and assgn their Value property to the real card in inventory for later process
            foreach (AdventCard card in cardSource)
            {
                AdventCard copy = InstantiateCard(card, new CardResource(card.Resource));
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
            if (result.Status == ChoiceStatus.Confirmed)
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
            List<AdventCard> cardPool = m_inventory.FilterInventory(filter) as List<AdventCard>;
            List<AdventCard> cardsToAdd = SamplingUtil.SampleFromList(cardPool, numToGet);
            m_hand.AddCardsToHand(cardsToAdd);
        }
        public void AddToInventory(List<AdventCard> add)
        {
            List<AdventCard> cardInstances = InstantiateCards(add);
            m_inventory.AddRange(cardInstances);
        }
        public void AddToInventory_FromAsset(List<CardAsset> cardsToDraw)
        {
            List<AdventCard> cardInstances = InstantiateCards_FromAsset(cardsToDraw);
            m_inventory.AddRange(cardInstances);
        }
        // Instantiate cards and add to hand
        public void AddToHand(List<AdventCard> cardsToDraw)
        {
            List<AdventCard> cardInstances = InstantiateCards(cardsToDraw);
            m_hand.AddCardsToHand(cardInstances);
        }
        public void AddToHand_FromAsset(List<CardAsset> cardsToDraw)
        {
            List<AdventCard> cardInstances = InstantiateCards_FromAsset(cardsToDraw);
            m_hand.AddCardsToHand(cardInstances);
        }
        public List<AdventCard> InstantiateCards_FromAsset(List<CardAsset> refs, CardInteractMode interactMode = CardInteractMode.Inspect) 
        {
            List<CardResource> assets = new List<CardResource>();
            foreach (var item in refs)
            {
                var resource = item.GetResource();
                assets.Add(resource);
            }
            List<AdventCard> ret = new List<AdventCard>();
            foreach (CardResource cardRef in assets)
            {
                // Instantiating cards to be drawn
                AdventCard cardInstance = InstantiateCard(m_cardRef, cardRef, interactMode);
                ret.Add(cardInstance);
            }
            return ret;
        }
        List<AdventCard> InstantiateCards(List<AdventCard> refs, CardInteractMode interactMode = CardInteractMode.Inspect)
        {
            List<CardResource> assets = new List<CardResource>();
            foreach (var item in refs)
            {
                assets.Add(item.Resource);
            }
            List<AdventCard> ret = new List<AdventCard>();
            foreach (CardResource cardRef in assets)
            {
                // Instantiating cards to be drawn
                AdventCard cardInstance = InstantiateCard(m_cardRef, cardRef, interactMode);
                ret.Add(cardInstance);
            }
            return ret;
        }
        AdventCard InstantiateCard(AdventCard cardRef, CardResource resource, CardInteractMode interactMode = CardInteractMode.Inspect)
        {
            AdventCard ret;
            ret = m_instance.GetInstanceFromAsset(cardRef.gameObject);
            ret.GetComponent<CardInteractionController>()?.Init(ret, interactMode);
            ret.InitResource(resource);
            return ret;
        }
        void OnAdventLoadFinish()
        {
            List<AdventCard> addToInventory = new List<AdventCard>();
            foreach (var advent in m_startingInventoryToLoad)
            {
                AdventCard newCard = InstantiateCard(m_cardRef, advent.GetResource());
                addToInventory.Add(newCard);
            }
            AddToInventory(addToInventory);
            IsReady = true;
        }
    }
}
