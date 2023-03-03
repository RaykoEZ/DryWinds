using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    // Triggers card behaviours when they are used (cooldown, consumable etc)
    public class PostCardActivationHandler : MonoBehaviour 
    {
        public IEnumerator OnCardUse(AdventCard used) 
        {
            if (used is ICooldown cd) 
            {
                yield return StartCoroutine(HandCooldown(cd));
            }
            // Consumable may deallocate card when it runs out of uses
            if (used is IConsumable consume) 
            {
                yield return StartCoroutine(HandleConsumable(used, consume));
            }
            yield return new WaitForEndOfFrame();
        }
        protected virtual IEnumerator HandCooldown(ICooldown cd) 
        {
            cd?.TrggerCooldown();
            yield return null;
        }
        protected virtual IEnumerator HandleConsumable(AdventCard used, IConsumable consume)
        {
            yield return consume?.OnExpend();
            yield return new WaitForEndOfFrame();
            used?.ReturnToPool();
        }
    }
}