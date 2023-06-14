using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Events;
using Curry.Util;
using Curry.UI;

namespace Curry.Explore
{
    public delegate void OnActionStart(int timeSpent, List<IEnumerator> onActivate = null);
    // Intermediary between cards-in-hand and main play zone
    // Handles card activations
    public partial class HandManager : MonoBehaviour
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
        protected Hand m_hand;
        public event OnActionStart OnActivate;
        public event OnCardReturn OnReturnToInventory;
        protected void Awake()
        {
            m_hand = new Hand(m_maxHandCapacity);
        }
        protected void Start()
        {
            m_onCardDraw?.Init();
            m_postActivation.Init(m_time, m_hand);
            m_postActivation.OnReturnToHand += AddCardsToHand;
            m_postActivation.OnReturnToInventory += ReturnCardsToInventory;
            // get starting hand
            DraggableCard[] cards = m_cardHolderRoot.GetComponentsInChildren<DraggableCard>();
            foreach (DraggableCard card in cards)
            {
                PrepareCard(card);
            }
            m_hand.AddRange(cards);
            m_spacing.UpdateSpacing();
        }
        #region Adding cards to hand
        public void AddCardsToHand(List<AdventCard> cards) 
        {
            List<DraggableCard> cardsToAdd = new List<DraggableCard>();
            DraggableCard drag;
            foreach (AdventCard card in cards)
            {
                drag = card.GetComponent<DraggableCard>();
                cardsToAdd.Add(drag);
                card.transform.SetParent(transform, false);
                PrepareCard(drag);
            }
            m_hand.AddRange(cardsToAdd);
            m_spacing.UpdateSpacing();
        }
        protected virtual void PrepareCard(DraggableCard draggable)
        {
            if (draggable == null)
            {
                return;
            }
            draggable.OnReturn += m_drag.OnCardReturn;
            draggable.OnDragBegin += m_drag.TargetGuide;
        }
        public void ReturnCardsToInventory(List<AdventCard> cards) 
        {
            foreach(var card in cards) 
            {
                OnCardLeavesHand(card.GetComponent<DraggableCard>());
            }
            OnReturnToInventory?.Invoke(cards);
        }
        public void OnCardDrawn(EventInfo info)
        {
            if (info == null) return;
            if (info is CardDrawInfo draw)
            {
                AddCardsToHand(draw.CardsDrawn as List<AdventCard>);
            }
        }
        protected virtual void OnCardLeavesHand(DraggableCard draggable)
        {
            if (draggable == null) return;
            draggable.OnDragBegin -= m_drag.TargetGuide;
            draggable.OnReturn -= m_drag.OnCardReturn;
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
            OnCardLeavesHand(draggable);
            m_drag.HideDropZone();
            yield return StartCoroutine(m_hand.PlayCard(draggable, m_player));
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