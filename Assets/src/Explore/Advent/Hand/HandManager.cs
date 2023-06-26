using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Assets.src.UI;

namespace Curry.Explore
{
    public delegate void OnActionStart(int timeSpent, List<IEnumerator> onActivate = null);
    // Intermediary between cards-in-hand and main play zone
    // Handles card activations
    public class HandManager : MonoBehaviour
    {
        [SerializeField] int m_maxHandCapacity = default;
        [SerializeField] Adventurer m_player = default;
        [SerializeField] TimeManager m_time = default;
        [SerializeField] PlayZone m_playZone = default;
        [SerializeField] Transform m_cardHolderRoot = default;
        [SerializeField] CurryGameEventListener m_onCardDraw = default;
        [SerializeField] PostCardActivationHandler m_postActivation = default;
        [SerializeField] LayoutSpaceSetting m_spacing = default;
        [SerializeField] CardDragHandler m_drag = default;
        [SerializeField] HandCapacityDisplay m_capacity = default;
        [SerializeField] CardAudioHandler m_audio = default;
        protected Hand m_hand;
        public int TotalHandHoldingValue => m_hand.TotalHandHoldingValue;
        public int MaxCapacity => m_hand.MaxCapacity;
        public IEnumerable<AdventCard> CardsInHand => new List<AdventCard>(m_hand.CardsInHand);

        public event OnActionStart OnActivate;
        public event OnCardReturn OnReturnToInventory;
        protected void Awake()
        {
            m_hand = new Hand(m_maxHandCapacity, m_drag);
        }
        protected void Start()
        {
            m_onCardDraw?.Init();
            m_postActivation.Init(m_time, m_hand);
            m_postActivation.OnReturnToHand += AddCardsToHand;
            m_postActivation.OnReturnToInventory += ReturnCardsToInventory;
            // get starting hand
            AdventCard[] cards = m_cardHolderRoot.GetComponentsInChildren<AdventCard>();
            m_hand.AddCards(cards);
            m_spacing.UpdateSpacing();
            m_capacity.UpdateDisplay(m_hand.MaxCapacity, m_hand.TotalHandHoldingValue);
        }
        #region Adding cards to hand
        public void AddCardsToHand(List<AdventCard> cards) 
        {
            List<AdventCard> cardsToAdd = new List<AdventCard>();
            foreach (AdventCard card in cards)
            {
                // If card is on cool down, apply cooldown tracking
                m_postActivation.TryApplyCoolDown(card);
                cardsToAdd.Add(card);
                card.transform.SetParent(transform, false);
            }
            m_hand.AddCards(cardsToAdd);
            m_audio?.OnCardDraw(cardsToAdd.Count);
            m_spacing.UpdateSpacing();
            m_capacity.UpdateDisplay(m_hand.MaxCapacity, m_hand.TotalHandHoldingValue);
        }
        public List<AdventCard> TakeCards(List<AdventCard> take) 
        {
            var ret = m_hand.TakeCards(take);
            m_spacing.UpdateSpacing();
            m_capacity.UpdateDisplay(m_hand.MaxCapacity, m_hand.TotalHandHoldingValue);
            return ret;
        }
        protected void ReturnCardsToInventory(List<AdventCard> cards) 
        {
            foreach(var card in cards) 
            {
                m_hand.TakeCard(card);
            }
            OnReturnToInventory?.Invoke(cards);
            m_capacity.UpdateDisplay(m_hand.MaxCapacity, m_hand.TotalHandHoldingValue);
        }
        public void OnCardDrawn(EventInfo info)
        {
            if (info == null) return;
            if (info is CardDrawInfo draw)
            {
                AddCardsToHand(draw.CardsDrawn as List<AdventCard>);
            }
        }
        protected virtual void OnCardLeavesHandParent(DraggableCard draggable)
        {
            m_hand.OnCardLeaveHand(draggable);
            // If card is dragged out of hand, we re calculate spacing
            m_spacing.UpdateSpacing();
        }
        #endregion
        #region Playing a card
        IEnumerator PlayCard(AdventCard card)
        {
            // Reset pending card
            m_drag.ResetDragTarget();
            DraggableCard draggable = card.GetComponent<DraggableCard>();
            OnCardLeavesHandParent(draggable);
            m_drag.HideDropZone();
            yield return StartCoroutine(m_hand.PlayCard(card, m_player));
            // after effect activation, we spend the card
            yield return StartCoroutine(m_postActivation.OnCardUse(card));
        }
        // When card is trying to actvated after it is dropped...
        void OnCardPlay(AdventCard card, Action onPlay, Action onCancel)
        {
            bool enoughTime = card.TimeCost <= m_time.TimeLeftToClear;
            //Try Spending Time/Resource, if not able, cancel
            if (!card.Activatable || !enoughTime)
            {
                m_drag.HideDropZone();
                onCancel?.Invoke();
            }else
            {
                m_time.TrySpendTime(card.TimeCost);
                onPlay?.Invoke();
                // Make a container for the callstack and trigger it. 
                List<IEnumerator> actions = new List<IEnumerator>
                {
                    PlayCard(card)
                };
                OnActivate?.Invoke(card.TimeCost, actions);
            }
        }
        #endregion
        #region for displaying/disabling UI for playing cards
        public void EnablePlay()
        {
            m_hand.EnablePlay();
            m_playZone.OnDropped += OnCardPlay;
            m_drag.OnCardTargetResolve += OnCardPlay;
        }
        public void DisablePlay()
        {
            m_hand.DisablePlay();
            m_playZone.OnDropped -= OnCardPlay;
            m_drag.OnCardTargetResolve -= OnCardPlay;
        }
        #endregion
    }
}