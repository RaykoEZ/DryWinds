using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Assets.src.UI;
using Curry.UI;
using System.Linq;
using Curry.Util;

namespace Curry.Explore
{
    public delegate void OnActionStart(ActionCost resouceSpent, List<IEnumerator> onActivate = null);
    // Intermediary between cards-in-hand and main play zone
    // Handles card activations
    public class HandManager : MonoBehaviour
    {
        [SerializeField] GameMessageTrigger m_abilityMessage = default;
        [SerializeField] Adventurer m_player = default;
        [SerializeField] ActionCostHandler m_cost = default;
        [SerializeField] PlayZone m_playZone = default;
        [SerializeField] Transform m_cardHolderRoot = default;
        [SerializeField] CurryGameEventListener m_onCardDraw = default;
        [SerializeField] PostCardActivationHandler m_postActivation = default;
        [SerializeField] LayoutSpaceSetting m_spacing = default;
        [SerializeField] CardActivationHandler m_activation = default;
        [SerializeField] HandCapacityDisplay m_capacity = default;
        [SerializeField] CardAudioHandler m_audio = default;
        [SerializeField] TurnEnd m_turnEnd = default;
        [SerializeField] protected Hand m_hand = default;
        public int TotalHandHoldingValue => m_hand.TotalHandHoldingValue;
        public int MaxCapacity => m_hand.MaxCapacity;
        public bool IsHandOverloaded => m_hand.IsHandOverloaded;
        public IEnumerable<AdventCard> CardsInHand => new List<AdventCard>(m_hand.CardsInHand);
        public event OnActionStart OnActivate;
        public event OnCardReturn OnReturnToInventory;
        protected void Start()
        {
            m_onCardDraw?.Init();
            m_postActivation.OnReturnToHand += PrepareCards;
            m_postActivation.OnReturnToInventory += ReturnCardsToInventory;
            m_postActivation.OnTakeFromHand += TakeCards;
            m_turnEnd.OnTurnEnding += OnTurnEndEffect;
            // get starting hand
            AdventCard[] cards = m_cardHolderRoot.GetComponentsInChildren<AdventCard>();
            m_hand.AddCards(cards);
            m_spacing.UpdateSpacing();
            m_capacity.UpdateDisplay(m_hand.MaxCapacity, m_hand.TotalHandHoldingValue);
        }
        #region Adding cards to hand
        public void AddCardsToHand(List<AdventCard> cards) 
        {
            PrepareCards(cards);
            m_hand.AddCards(cards);
            m_audio?.OnCardDraw(cards.Count);
            m_spacing.UpdateSpacing();
            m_capacity.UpdateDisplay(m_hand.MaxCapacity, m_hand.TotalHandHoldingValue);
        }
        // Initialize card handles and activate effect triggers on hand 
        void PrepareCards(List<AdventCard> toPrep)
        {
            foreach (AdventCard card in toPrep)
            {
                PrepareCard(card);
            }
        }
        void PrepareCard(AdventCard card) 
        {
            if (card == null)
            {
                return;
            }           
            if (card.Resource is IHandEffect handEffect) 
            {
                m_abilityMessage.TriggerGameMessage(card.Resource.Name, Color.blue);
                m_activation.ActivateHandEffect(handEffect);
            }
            else 
            {
                card.GetComponent<DraggableCard>().OnReturn += m_activation.OnCardReturn;
                card.GetComponent<DraggableCard>().OnDragBegin += m_activation.TargetGuide;
            }
            // If card is on cool down, apply cooldown tracking
            m_postActivation.ApplyCoolDown(card);
            card.transform.SetParent(transform, false);
            card.GetComponent<CardInteractionController>()?.SetInteractionMode(
                CardInteractMode.Play | CardInteractMode.Inspect);
        }
        void OnTurnEndEffect() 
        {
            StartCoroutine(TurnEndEffect_Internal());
        }
        IEnumerator TurnEndEffect_Internal() 
        {
            foreach (AdventCard card in CardsInHand)
            {
                if (card.Resource is IEndOfTurnEffect effect)
                {
                    m_abilityMessage.TriggerGameMessage(card.Resource.Name, Color.blue);
                    yield return m_activation.EndOfTurnEffect(effect);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        public List<AdventCard> TakeCards(List<AdventCard> take) 
        {
            var ret = m_hand.TakeCards(take);
            foreach (AdventCard taken in ret) 
            {
                OnCardLeaveHand(taken.GetComponent<DraggableCard>());
            }
            m_spacing.UpdateSpacing();
            m_capacity.UpdateDisplay(m_hand.MaxCapacity, m_hand.TotalHandHoldingValue);
            return ret;
        }
        protected virtual void OnCardLeaveHand(DraggableCard drag)
        {
            if (drag == null) return;

            if (drag.Card.Resource is IHandEffect handEffect) 
            {
                m_activation.RevertHandEffect(handEffect);
            }
            drag.OnDragBegin -= m_activation.TargetGuide;
            drag.OnReturn -= m_activation.OnCardReturn;
        }
        protected void ReturnCardsToInventory(List<AdventCard> cards) 
        {
            var take = m_hand.TakeCards(cards);
            OnReturnToInventory?.Invoke(take);
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
        //Discard a card with specified card name
        public void DiscardCardByName(string name) 
        {
            foreach (var item in m_hand.CardsInHand)
            {
                if (item.Resource.Name == name) 
                {
                    m_hand.TakeCard(item);
                    m_postActivation.RemoveFromCooldownUpdate(new List<AdventCard> { item });
                    item.ReturnToPool();
                    break;
                }
            }
        }
        // discard random card(s) in hand
        public void DiscardRandom(int numToDiscard = 1) 
        {
            List<AdventCard> toDiscard = SamplingUtil.SampleFromList(m_hand.CardsInHand.ToList(), numToDiscard);
            DiscardCards(toDiscard);
        }
        // discard all card in hand
        public void DiscardAll()
        {
            DiscardCards(m_hand.CardsInHand.ToList());
        }

        void DiscardCards(List<AdventCard> cards)
        {
            var toDiscard = TakeCards(cards);
            m_postActivation.RemoveFromCooldownUpdate(toDiscard);
            foreach (var item in toDiscard)
            {
                item.ReturnToPool();
            }
        }
        public void TriggerAllCooldowns()
        {
            foreach (var item in m_hand.CardsInHand)
            {
                m_postActivation.ApplyCoolDown(item, forceApply: true);
            }
        }
        protected virtual void OnCardLeavesHandParent(DraggableCard draggable)
        {
            OnCardLeaveHand(draggable);
            // If card is dragged out of hand, we re calculate spacing
            m_spacing.UpdateSpacing();
        }
        #endregion
        #region Playing a card
        IEnumerator PlayCard(GameStateContext c, AdventCard card)
        {
            m_cost.TrySpend(card.Resource.Cost);
            m_abilityMessage.TriggerGameMessage(card.Resource.Name, Color.blue);
            // Reset pending card
            m_activation.ResetDragTarget();
            DraggableCard draggable = card.GetComponent<DraggableCard>();
            OnCardLeavesHandParent(draggable);
            m_activation.HideDropZone();
            yield return m_hand.PlayCard(c, card, m_player);
            // after effect activation, we spend the card
            if (card.Resource is IConsumable) 
            {
                m_audio?.OnCardConsume();
            }
            yield return m_postActivation.OnCardUse(card, IsHandOverloaded);
        }
        // When card is trying to actvated after it is dropped...
        void OnCardPlay(GameStateContext c, AdventCard card, Action onPlay, Action onCancel)
        {
            bool enoughTime = m_cost.HasEnoughResource(card.Resource.Cost, true);
            //Try Spending Time/Resource, if not able, cancel
            if (!card.IsActivatable(c) || !enoughTime || card.Resource.IsOnCooldown)
            {
                m_activation.HideDropZone();
                onCancel?.Invoke();
            }
            else
            {
                m_cost.CancelPreview();
                onPlay?.Invoke();
                // Make a container for the callstack and trigger it. 
                List<IEnumerator> actions = new List<IEnumerator>
                {
                    PlayCard(c, card)
                };
                OnActivate?.Invoke(card.Resource.Cost, actions);
            }
        }
        #endregion
        #region for displaying/disabling UI for playing cards
        public void EnablePlay()
        {
            foreach (AdventCard card in m_hand.CardsInHand)
            {
                card.GetComponent<CardInteractionController>()?.SetInteractionMode(
                    CardInteractMode.Play | CardInteractMode.Inspect);
            }
            m_playZone.OnPlayed += OnCardPlay;
            m_activation.OnCardTargetResolve += OnCardPlay;
        }
        public void DisablePlay()
        {
            foreach (AdventCard card in m_hand.CardsInHand)
            {
                card.GetComponent<CardInteractionController>()?.
                    SetInteractionMode(CardInteractMode.Inspect);
            }
            m_playZone.OnPlayed -= OnCardPlay;
            m_activation.OnCardTargetResolve -= OnCardPlay;
        }
        #endregion
    }
}