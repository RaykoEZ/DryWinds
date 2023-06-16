using System.Collections.Generic;
using UnityEngine;
using Curry.Util;
using TMPro;
using UnityEngine.Rendering;
using System;
using Curry.UI;
using System.Linq;

namespace Curry.Explore
{
    public delegate void OnReformulateFinish(List<AdventCard> inventoryToHand, List<AdventCard> handToInventory);
    public class ReformulateUIHandler : MonoBehaviour 
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_handCapacityField = default;
        [SerializeField] CardDropZone m_inventoryZone = default;
        [SerializeField] CardDropZone m_handZone = default;
        public bool IsDisplaying { get; protected set; }
        HashSet<AdventCard> m_inventoryToHand = new HashSet<AdventCard>();
        HashSet<AdventCard> m_handToInventory = new HashSet<AdventCard>();
        Hand m_handRef;
        Inventory m_inventoryRef;
        int m_handCapacity = 0;
        int m_handHoldingValue = 0;
        void Start()
        {
            m_inventoryZone.OnDropped += DropToInventory;
            m_handZone.OnDropped += DropToHand;
            AdventCard[] inv = m_inventoryZone.transform.GetComponentsInChildren<AdventCard>();
            AdventCard[] hand = m_handZone.transform.GetComponentsInChildren<AdventCard>();
            PrepareCards(new List<AdventCard>(inv));
            PrepareCards(new List<AdventCard>(hand));
        }
        public void Show(Hand hand, Inventory inventory) 
        {
            IsDisplaying = true;
            ResetHandler();
            m_inventoryRef = inventory;
            m_handRef = hand;
            m_handCapacity = m_handRef.MaxCapacity;
            m_handHoldingValue = m_handRef.TotalHandHoldingValue;
            UpdateCapacityDisplay();
            PrepareCards(hand.CardsInHand as List<AdventCard>);
            PrepareCards(inventory.CardsInStock as List<AdventCard>);
            m_anim?.SetBool("show", true);
        }
        public void Hide()
        {
            IsDisplaying = false;
            m_anim?.SetBool("show", false);
        }
        public void Finish()
        {
            // Move cards between hand and inventory according to lists of additions
            ResetHandler();
            Hide();
        }
        public void Cancel()
        {
            ResetHandler();
            Hide();
        }
        void ResetHandler() 
        {
            m_inventoryToHand.Clear();
            m_handToInventory.Clear();
        }
        void UpdateCapacityDisplay() 
        {
            m_handCapacityField.text = $"{m_handHoldingValue} / {m_handCapacity}"; 
        }
        void PrepareCards(List<AdventCard> cards) 
        { 
            foreach(AdventCard card in cards) 
            {
                card?.GetComponent<CardInteractionController>()?.
                    SetInteractionMode(CardInteractMode.Play | CardInteractMode.Inspect);
            //TODO: Add on drag begin handles to show drop zones (hand/inventory)
            }
        }
        void DropToInventory(AdventCard drop, Action onDrop, Action onCancel) 
        {
            // Check if the dropped card exists in starting hand [OR] edited hand
            bool doesDropExistInHand = m_inventoryToHand.Contains(drop) ||
                (m_handRef.ContainsCard(drop) && !m_handToInventory.Contains(drop));
            // don't add duplicate card when cancelling drag and drop
            if (doesDropExistInHand &&
                !m_inventoryRef.ContainsCard(drop)) 
            {
                m_inventoryToHand.Remove(drop);
                m_handToInventory.Add(drop);
                onDrop?.Invoke();
            }
            else 
            {
                onCancel?.Invoke();
            }
        }
        void DropToHand(AdventCard drop, Action onDrop, Action onCancel) 
        {
            // Check if the dropped card exists in starting inventory [OR] edited inventory
            bool doesDropExistInInventory = m_handToInventory.Contains(drop) ||
                (m_inventoryRef.ContainsCard(drop) && !m_inventoryToHand.Contains(drop));
            // don't add duplicate card into addList when cancelling drag and drop
            if (doesDropExistInInventory && 
                !m_handRef.ContainsCard(drop)) 
            {
                m_handToInventory.Remove(drop);
                m_inventoryToHand.Add(drop);
                onDrop?.Invoke();
            }
            else 
            {
                onCancel?.Invoke();
            }
        }
    }
}