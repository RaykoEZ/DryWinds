using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    // Triggers card behaviours when they are used (cooldown, consumable etc)
    public class PostCardActivationHandler : MonoBehaviour 
    {
        [SerializeField] TimeManager m_time = default;
        List<ICooldown> m_cooldowns = new List<ICooldown>();
        void Start()
        {
            m_time.OnTimeSpent += OnCooldownTick;
        }
        public IEnumerator OnCardUse(AdventCard used) 
        {
            if (used is ICooldown cd) 
            {
                yield return StartCoroutine(HandleCooldown(cd));
            }
            // Consumable may deallocate card when it runs out of uses
            if (used is IConsumable consume) 
            {
                yield return StartCoroutine(HandleConsumable(used, consume));
            }
            yield return new WaitForEndOfFrame();
        }
        protected void OnCooldownTick(int spent, int timeLeft) 
        {
            foreach (ICooldown cd in m_cooldowns) 
            {
                cd.Tick(spent, out _);
            }
        }
        protected virtual IEnumerator HandleCooldown(ICooldown cd) 
        {
            m_cooldowns.Add(cd);
            cd?.TrggerCooldown();
            yield return null;
        }
        protected virtual IEnumerator HandleConsumable(AdventCard used, IConsumable consume)
        {
            yield return consume?.OnExpend();
            yield return new WaitForEndOfFrame();
            if (consume is ICooldown cd) 
            { 
                m_cooldowns.Remove(cd); 
            }
            used?.ReturnToPool();
        }
    }
}