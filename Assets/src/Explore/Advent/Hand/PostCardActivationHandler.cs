using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
        public void TryApplyCoolDown(AdventCard card) 
        {
            if (card is ICooldown cd && 
                cd.IsOnCooldown &&
                !m_cooldowns.Contains(card))
            {
                StartCoroutine(HandleCooldown(cd, card));
            }
        }
        public IEnumerator OnCardUse(AdventCard used, bool isHandOverloaded) 
        {
            if (used is ICooldown cd) 
            {
                yield return StartCoroutine(HandleCooldown(cd, used));
            }
            // Consumable may deallocate card when it runs out of uses.
            // If hand is overloaded when card is played:
            // (total holding value in hand > max hand capacity)
            // played card will go back to inventory
            List<AdventCard> take = new List<AdventCard> { used };
            if (used is IConsumable consume) 
            {
                OnTakeFromHand?.Invoke(take);
                yield return StartCoroutine(HandleConsumable(used, consume));
            }
            else if(isHandOverloaded)
            {
                m_cooldowns.Remove(used);
                OnReturnToInventory?.Invoke(take);
            }
            else 
            {
                used.GetComponent<DraggableCard>()?.ReturnToBeforeDrag();
            }
            yield return new WaitForEndOfFrame();
        }
        protected void OnCooldownTick(int spent, int timeLeft) 
        {
            List<AdventCard> cardsToReturn = new List<AdventCard>();
            bool isStillOnCooldown;
            ICooldown cd;
            foreach (AdventCard card in m_cooldowns) 
            {
                cd = card as ICooldown;
                cd.Tick(spent, out isStillOnCooldown);
                if (!isStillOnCooldown) 
                {
                    cardsToReturn.Add(card);
                }
            }
            foreach(AdventCard card in cardsToReturn) 
            {
                m_cooldowns.Remove(card);
            }
            OnReturnToHand?.Invoke(cardsToReturn);
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
            yield return new WaitForEndOfFrame();
            if (consume is ICooldown) 
            { 
                m_cooldowns.Remove(used); 
            }
            used?.ReturnToPool();
        }
    }
}