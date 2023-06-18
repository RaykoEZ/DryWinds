using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Curry.UI;

namespace Curry.Explore
{
    public delegate void OnReformulateFinish(List<AdventCard> inventoryToHand, List<AdventCard> handToInventory);
    public class ReformulateUIHandler : MonoBehaviour 
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_handCapacityField = default;
        [SerializeField] PlayZone m_inventoryZone = default;
        [SerializeField] PlayZone m_handZone = default;
        public bool IsDisplaying { get; protected set; }
        public bool IsHandOverloaded => m_handHoldingValue > m_handCapacity;
        List<AdventCard> m_handCards = new List<AdventCard>();
        List<AdventCard> m_inventoryCards = new List<AdventCard>();
        HandManager m_handRef;
        Inventory m_inventoryRef;
        int m_handCapacity = 0;
        int m_handHoldingValue = 0;
        void Start()
        {
            m_inventoryZone.OnDropped += DropToInventory;
            m_handZone.OnDropped += DropToHand;
            AdventCard[] inv = m_inventoryZone.transform.GetComponentsInChildren<AdventCard>();
            AdventCard[] hand = m_handZone.transform.GetComponentsInChildren<AdventCard>();
            m_inventoryCards.AddRange(inv);
            m_handCards.AddRange(hand);
            PrepareCards(new List<AdventCard>(inv));
            PrepareCards(new List<AdventCard>(hand));
        }
        public void Show(HandManager hand, Inventory inventory) 
        {
            IsDisplaying = true;
            ResetHandler();
            m_inventoryRef = inventory;
            m_handRef = hand;
            m_handCapacity = m_handRef.HandContent.MaxCapacity;
            m_handHoldingValue = m_handRef.HandContent.TotalHandHoldingValue;
            UpdateCapacityDisplay();
            var hands = hand.HandContent.TakeCards(new List<AdventCard>(hand.HandContent.CardsInHand));
            PrepareCards(hands);
            CardToViewer(m_handZone, hands);
            var inv = inventory.TakeCards(new List<AdventCard>(inventory.CardsInStock));
            PrepareCards(inv);
            m_inventoryCards.AddRange(inv);
            m_handCards.AddRange(hands);
            CardToViewer(m_inventoryZone, inv);
            m_anim?.SetBool("show", true);
        }
        void CardToViewer(PlayZone zone, List<AdventCard> cards) 
        { 
            foreach(var card in cards) 
            {
                card.transform.SetParent(zone.transform, false);
            }
        }
        public void Hide()
        {
            IsDisplaying = false;
            m_anim?.SetBool("show", false);
        }
        public void Finish()
        {
            // Move cards between hand and inventory according to lists of additions
            // Show error if player tries to finish when result hand capacity is overloaded
            if (IsHandOverloaded) 
            {
                Debug.LogWarning($"Hand Capacity overloaded ({m_handHoldingValue} / {m_handCapacity})");
            }
            else 
            {
                ResolveHand();
                ResolveInventory();
                ResetHandler();
                Hide();
            }
        }
        void ResolveHand() 
        {
            foreach (AdventCard card in m_handCards)
            {
                card.GetComponent<DraggableCard>().OnDragBegin -= OnCardDrag;
                card.GetComponent<DraggableCard>().OnDragFinish -= OnDragEnd;
            }
            m_handRef.AddCardsToHand(m_handCards);
        }
        void ResolveInventory() 
        {
            foreach (AdventCard card in m_inventoryCards)
            {
                card.GetComponent<DraggableCard>().OnDragBegin -= OnCardDrag;
                card.GetComponent<DraggableCard>().OnDragFinish -= OnDragEnd;
            }
            m_inventoryRef.AddRange(m_inventoryCards);
        }
        public void End()
        {
            //TODO: need to reset to original hand & inventory state
            ResetHandler();
            Hide();
        }
        void ResetHandler() 
        {
            m_handCards.Clear();
            m_inventoryCards.Clear();
        }
        void UpdateCapacityDisplay() 
        {
            m_handCapacityField.text = IsHandOverloaded?
            $" <color=red>{m_handHoldingValue} / {m_handCapacity}</color>" :
            $"{m_handHoldingValue} / {m_handCapacity}";
        }
        void PrepareCards(List<AdventCard> cards) 
        {
            if (cards == null) return;
            foreach(AdventCard card in cards) 
            {
                card.GetComponent<CardInteractionController>()?.
                    SetInteractionMode(CardInteractMode.Move | CardInteractMode.Inspect);
                var drag = card.GetComponent<DraggableCard>();
                drag.OnDragBegin += OnCardDrag;
                drag.OnDragFinish += OnDragEnd;
            }
        }
        void OnDragEnd(DraggableCard card)
        {
            m_inventoryZone.SetPlayZonrActive(false);
            m_handZone.SetPlayZonrActive(false);

        }
        void OnCardDrag(DraggableCard drag) 
        {
            AdventCard card = drag.GetComponent<AdventCard>();
            if (m_handCards.Contains(card)) 
            {
                m_inventoryZone.SetPlayZonrActive();
            }
            else if (m_inventoryCards.Contains(card)) 
            {
                m_handZone.SetPlayZonrActive();
            }
        }
        void DropToInventory(AdventCard drop, Action onDrop, Action onCancel) 
        {
            // Check if the dropped card exists in starting hand [OR] edited hand
            // don't add duplicate card when cancelling drag and drop
            if (m_handCards.Contains(drop)) 
            {
                m_handCards.Remove(drop);
                m_inventoryCards.Add(drop);
                m_handHoldingValue -= drop.HoldingValue;
                onDrop?.Invoke();
                UpdateCapacityDisplay();
            }
            else 
            {
                onCancel?.Invoke();
            }
        }
        void DropToHand(AdventCard drop, Action onDrop, Action onCancel) 
        {
            // Check if the dropped card exists in starting inventory [OR] edited inventory
            // don't add duplicate card into addList when cancelling drag and drop
            if (m_inventoryCards.Contains(drop)) 
            {
                m_inventoryCards.Remove(drop);
                m_handCards.Add(drop);
                m_handHoldingValue += drop.HoldingValue;
                onDrop?.Invoke();
                UpdateCapacityDisplay();
            }
            else 
            {
                onCancel?.Invoke();
            }
        }
    }
}