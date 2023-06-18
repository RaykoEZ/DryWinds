using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Curry.UI;

namespace Curry.Explore
{
    public class ReformulateState 
    {
        public List<AdventCard> HandCards;
        public List<AdventCard> InventoryCards;
        public void Clear() 
        {
            HandCards.Clear();
            InventoryCards.Clear();
        }
        public ReformulateState() 
        {
            HandCards = new List<AdventCard>();
            InventoryCards = new List<AdventCard>();
        }
        public ReformulateState(ReformulateState copy) 
        {
            HandCards = new List<AdventCard>(copy.HandCards);
            InventoryCards = new List<AdventCard>(copy.InventoryCards);
        }
    }

    public delegate void OnReformulateFinish(List<AdventCard> inventoryToHand, List<AdventCard> handToInventory);
    public class ReformulateUIHandler : MonoBehaviour 
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_handCapacityField = default;
        [SerializeField] PlayZone m_inventoryZone = default;
        [SerializeField] PlayZone m_handZone = default;
        public bool IsDisplaying { get; protected set; }
        public bool IsHandOverloaded => m_handHoldingValue > m_handCapacity;
        ReformulateState m_original = new ReformulateState();
        ReformulateState m_current;
        HandManager m_handRef;
        Inventory m_inventoryRef;
        int m_handCapacity = 0;
        int m_handHoldingValue = 0;
        void Start()
        {
            m_inventoryZone.OnDropped += DropToInventory;
            m_handZone.OnDropped += DropToHand;
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
            m_original.InventoryCards.AddRange(inv);
            m_original.HandCards.AddRange(hands);
            m_current = new ReformulateState(m_original);
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
            foreach (AdventCard card in m_current.HandCards)
            {
                card.GetComponent<DraggableCard>().OnDragBegin -= OnCardDrag;
                card.GetComponent<DraggableCard>().OnDragFinish -= OnDragEnd;
            }
            m_handRef.AddCardsToHand(m_current.HandCards);
        }
        void ResolveInventory() 
        {
            foreach (AdventCard card in m_current.InventoryCards)
            {
                card.GetComponent<DraggableCard>().OnDragBegin -= OnCardDrag;
                card.GetComponent<DraggableCard>().OnDragFinish -= OnDragEnd;
            }
            m_inventoryRef.AddRange(m_current.InventoryCards);
        }
        public void End()
        {
            m_current = m_original;
            ResolveHand();
            ResolveInventory();
            ResetHandler();
            Hide();
        }
        void ResetHandler() 
        {
            m_current?.Clear();
            m_original?.Clear();
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
            if (m_current.HandCards.Contains(card)) 
            {
                m_inventoryZone.SetPlayZonrActive();
            }
            else if (m_current.InventoryCards.Contains(card)) 
            {
                m_handZone.SetPlayZonrActive();
            }
        }
        void DropToInventory(AdventCard drop, Action onDrop, Action onCancel) 
        {
            // Check if the dropped card exists in starting hand [OR] edited hand
            // don't add duplicate card when cancelling drag and drop
            if (m_current.HandCards.Contains(drop)) 
            {
                m_current.HandCards.Remove(drop);
                m_current.InventoryCards.Add(drop);
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
            if (m_current.InventoryCards.Contains(drop)) 
            {
                m_current.InventoryCards.Remove(drop);
                m_current.HandCards.Add(drop);
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