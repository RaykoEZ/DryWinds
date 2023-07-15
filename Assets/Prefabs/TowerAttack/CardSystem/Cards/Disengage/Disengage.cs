using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    
    public class Disengage : AdventCard, ICooldown
    {
        [SerializeField] Heal_EffectResource m_heal;
        [SerializeField] ExhaustAp_EffectResource m_exhaustActionPoint = default;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            m_exhaustActionPoint.Effect.ApplyEffect(context.ActionCount, out int numSpent);
            // heal for [base healing value] * num. of actions exhausted
            m_heal.Healing.ApplyEffect(context.Player, numSpent * m_heal.Healing.HealAmount);
            yield return null;
        }
    }
}