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
    // Used to dynamically set layout group spacing depending on number of items in the horizontal layout group
    [Serializable]
    public class LayoutSpaceSetting
    {
        [SerializeField] float m_minSpacing = default;
        [SerializeField] float m_maxSpacing = default;
        [SerializeField] float itemWidth = default;
        [SerializeField] HorizontalLayoutGroup m_layout = default;
        [SerializeField] RectTransform m_layoutLimit = default;

        // Used to calculate the ideal spacing for cards in a horizontal list of cards
        public void UpdateSpacing()
        {
            float numItem = m_layout.transform.childCount;
            float newSpacing = ((m_layoutLimit.rect.width - itemWidth) / (numItem - 1)) - itemWidth;
            newSpacing = Mathf.Clamp(newSpacing, m_minSpacing, m_maxSpacing);
            m_layout.spacing = newSpacing;
        }
    }

    public delegate void OnActionStart(int timeSpent, List<IEnumerator> onActivate = null);
    // Intermediary between cards-in-hand and main play zone
    // Handles card activations
    public partial class HandManager : CardDropZone
    {
        [SerializeField] Adventurer m_player = default;
        [SerializeField] TimeManager m_time = default;
        [SerializeField] CardDropZone m_playZone = default;
        [SerializeField] Transform m_cardHolderRoot = default;
        [SerializeField] CurryGameEventListener m_onCardDraw = default;
        [SerializeField] CurryGameEventListener m_onDiscardHand = default;
        [SerializeField] CurryGameEventListener m_onDropTileSelected = default;
        [SerializeField] Image m_playPanel = default;
        [SerializeField] SelectionManager m_selection = default;
        [SerializeField] PostCardActivationHandler m_postActivation = default;
        [SerializeField] LayoutSpaceSetting m_spacing = default;
        // The card we are dragging into a play zone
        DraggableCard m_pendingCardRef;
        protected Hand m_cardsInHand = new Hand();
        // When a card, that targets a position, finishes targeting...
        protected event OnCardDrop OnCardTargetResolve;
        public event OnActionStart OnActivate;
        protected void Start()
        {
            m_onDropTileSelected?.Init();
            m_onCardDraw?.Init();
            m_onDiscardHand?.Init();
            m_postActivation.Init(m_time);
            m_postActivation.OnCardReturn += AddCardsToHand;
            // get starting hand
            DraggableCard[] cards = m_cardHolderRoot.GetComponentsInChildren<DraggableCard>();
            foreach (DraggableCard card in cards)
            {
                PrepareCard(card);
                m_cardsInHand.Add(card);
            }
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
                card.GetComponent<CardInteractionController>()?.
                    SetInteractionMode(CardInteractMode.Play | CardInteractMode.Inspect);

                cardsToAdd.Add(drag);
                card.transform.SetParent(transform, false);
                PrepareCard(drag);
            }
            m_cardsInHand.AddRange(cardsToAdd);
            m_spacing.UpdateSpacing();
        }
        protected virtual void PrepareCard(DraggableCard draggable)
        {
            if (draggable == null)
            {
                return;
            }
            draggable.OnReturn += OnCardReturn;
            draggable.OnDragBegin += TargetGuide;
        }

        public void OnCardDrawn(EventInfo info)
        {
            if (info == null) return;
            if (info is CardDrawInfo draw)
            {
                AddCardsToHand(draw.CardsDrawn as List<AdventCard>);
            }
        }
        #endregion
        #region Playing a card
        IEnumerator PlayCard(AdventCard card)
        {
            // Reset pending card
            m_pendingCardRef = null;
            DraggableCard draggable = card.GetComponent<DraggableCard>();
            OnCardLeavesHand(draggable);
            HidePlayZone();
            yield return StartCoroutine(m_cardsInHand.PlayCard(draggable, m_player));
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
                HidePlayZone();
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
            m_cardsInHand.EnablePlay();
            m_playZone.OnDropped += OnCardPlay;
            OnCardTargetResolve += OnCardPlay;
        }
        public void DisablePlay()
        {
            m_cardsInHand.DisablePlay();
            m_playZone.OnDropped -= OnCardPlay;
            OnCardTargetResolve -= OnCardPlay;
        }
        protected void ShowPlayZone()
        {
            if (m_pendingCardRef != null && m_pendingCardRef.DoesCardNeedTarget)
            {
                ITargetsPosition targetCard = m_pendingCardRef.Card as ITargetsPosition;
                m_selection.SelectDropZoneTile(m_pendingCardRef.Card.Name, targetCard.TargetingRange, m_player.transform);
            }
            else
            {
                m_playPanel.enabled = true;
            }
        }
        protected void HidePlayZone()
        {
            m_selection.CancelSelection();
            m_playPanel.enabled = false;
        }
        #endregion
        #region Cards leaves hand/ return to hand 
        protected virtual void OnCardLeavesHand(DraggableCard draggable)
        {
            draggable.OnDragBegin -= TargetGuide;
            draggable.OnReturn -= OnCardReturn;
            // If card is dragged out of hand, we re calculate spacing
            m_spacing.UpdateSpacing();
        }
        public void DiscardHand()
        {
            foreach (DraggableCard c in m_cardsInHand.CardsInHand)
            {
                OnCardLeavesHand(c);
            }
            HidePlayZone();
            m_cardsInHand.DiscardCards();
        }
        void OnCardReturn(DraggableCard card)
        {
            m_pendingCardRef = null;
            HidePlayZone();
        }
        #endregion
        #region Handles targeting UI when plating a card
        public void OnTargetDropZoneSelected(EventInfo info)
        {
            if (m_pendingCardRef == null) return;

            if (info is PositionInfo pos)
            {
                // Activate card effect with target
                ITargetsPosition handler = m_pendingCardRef.Card as ITargetsPosition;
                handler.SetTarget(pos.WorldPosition);
            }
            // do activation validation
            OnCardTargetResolve?.Invoke(m_pendingCardRef.Card, onDrop: null, onCancel: m_pendingCardRef.OnCancel);
        }
        protected virtual void TargetGuide(DraggableCard draggable)
        {
            if (draggable.DoesCardNeedTarget)
            {
                m_pendingCardRef = draggable;
                m_selection?.TargetGuide(m_pendingCardRef.transform);
            }
            ShowPlayZone();

        }
        #endregion
    }
}