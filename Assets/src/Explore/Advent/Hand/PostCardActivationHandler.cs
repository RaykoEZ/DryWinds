using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnCardReturn(List<AdventCard> toReturn);
    // Triggers card behaviours when they are used (cooldown, consumable etc)
    public class PostCardActivationHandler : MonoBehaviour 
    {
        protected TimeManager m_time;
        protected List<AdventCard> m_cooldowns = new List<AdventCard>();
        public event OnCardReturn OnCardReturn;
        public void Init(TimeManager time)
        {
            m_time = time;
            m_time.OnTimeSpent += OnCooldownTick;
        }
        public IEnumerator OnCardUse(AdventCard used) 
        {
            if (used is ICooldown cd) 
            {
                yield return StartCoroutine(HandleCooldown(cd, used));
            }
            // Consumable may deallocate card when it runs out of uses
            if (used is IConsumable consume) 
            {
                yield return StartCoroutine(HandleConsumable(used, consume));
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
            OnCardReturn?.Invoke(cardsToReturn);
        }
        protected virtual IEnumerator HandleCooldown(ICooldown cd, AdventCard card) 
        {
            m_cooldowns.Add(card);
            cd?.TrggerCooldown();
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