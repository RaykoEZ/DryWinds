using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{  
    public class SizzlingQuakespear : AdventCard, IConsumable
    {
        [SerializeField] DealDamage_EffectResource m_selfDamage = default;
        [SerializeField] ActionPointChange_EffectResource m_gainAction = default;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            m_selfDamage?.Activate(context);
            m_gainAction?.Activate(context);
            yield return null;
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}