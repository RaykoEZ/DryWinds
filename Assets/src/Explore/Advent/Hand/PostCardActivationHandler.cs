﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Curry.Explore
{
    public delegate List<AdventCard> OnTakeCardFromHand(List<AdventCard> take);
    public delegate void OnCardReturn(List<AdventCard> toReturn);
    // Triggers card behaviours when they are used (cooldown, consumable etc)
    public class PostCardActivationHandler : MonoBehaviour 
    {
        [SerializeField] TimeManager m_time = default;
        protected List<AdventCard> m_cooldowns = new List<AdventCard>();
        public event OnCardReturn OnReturnToHand;
        public event OnCardReturn OnReturnToInventory;
        public event OnTakeCardFromHand OnTakeFromHand;
        private void Start()
        {
            m_time.OnTimeSpent += OnCooldownTick;
        }
        void OnDestroy()
        {
            m_time.OnTimeSpent -= OnCooldownTick;
        }
        public void ApplyCoolDown(AdventCard card, bool forceApply = false) 
        {
            if (card.Resource is ICooldown cd &&
                (forceApply || cd.IsOnCooldown) &&
                !m_cooldowns.Contains(card))
            {
                StartCoroutine(HandleCooldown(cd, card));
            }
        }
        public IEnumerator OnCardUse(AdventCard used, bool isHandOverloaded) 
        {
            if (used.Resource is ICooldown cd) 
            {
                yield return StartCoroutine(HandleCooldown(cd, used));
            }
            // Consumable may deallocate card when it runs out of uses.
            // If hand is overloaded when card is played:
            // (total holding value in hand > max hand capacity)
            // played card will go back to inventory
            List<AdventCard> take = new List<AdventCard> { used };
            if (used.Resource is IConsumable consume) 
            {
                OnTakeFromHand?.Invoke(take);
                yield return StartCoroutine(HandleConsumable(used, consume));
            }
            else if(isHandOverloaded)
            {
                yield return StartCoroutine(ReturnToInventory_Internal(used));
            }
            else 
            {
                used.GetComponent<DraggableCard>()?.ReturnToBeforeDrag();
                OnReturnToHand?.Invoke(take);
            }
            yield return new WaitForEndOfFrame();
        }
        public void RemoveFromCooldownUpdate(List<AdventCard> cardsToClear)
        {
            foreach (AdventCard card in cardsToClear)
            {
                m_cooldowns.Remove(card);
            }
        }
        protected void OnCooldownTick(int spent, int timeLeft) 
        {
            List<AdventCard> cardsToReturn = new List<AdventCard>();
            bool isStillOnCooldown;
            ICooldown cd;
            foreach (AdventCard card in m_cooldowns) 
            {
                cd = card.Resource as ICooldown;
                cd.Tick(spent, out isStillOnCooldown);
                if (!isStillOnCooldown) 
                {
                    cardsToReturn.Add(card);
                }
            }
            RemoveFromCooldownUpdate(cardsToReturn);
            OnReturnToHand?.Invoke(cardsToReturn);
        }
        protected virtual IEnumerator ReturnToInventory_Internal(AdventCard card)
        {
            card?.GetComponent<Animator>()?.ResetTrigger("returnInventory");
            card?.GetComponent<Animator>()?.SetTrigger("returnInventory");
            yield return new WaitForSeconds(0.3f);
            if (card.Resource is ICooldown cd)
            {
                m_cooldowns.Remove(card);
            }
            OnReturnToInventory?.Invoke(new List<AdventCard>{card});
        }
        protected virtual IEnumerator HandleCooldown(ICooldown cd, AdventCard card) 
        {
            m_cooldowns.Add(card);
            cd?.TriggerCooldown();
            yield return null;
        }
        protected virtual IEnumerator HandleConsumable(AdventCard used, IConsumable consume)
        {
            yield return consume?.OnExpend();
            Animator anim = used?.GetComponent<Animator>();
            anim?.ResetTrigger("consume");
            anim?.SetTrigger("consume");
            yield return new WaitForSeconds(0.25f);
            anim?.ResetTrigger("consume");
            if (consume is ICooldown) 
            { 
                m_cooldowns.Remove(used); 
            }
            used?.ReturnToPool();
        }
    }
}