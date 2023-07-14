using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    
    public class Disengage : AdventCard
    {
        [SerializeField] Heal_EffectResource m_heal;
        [SerializeField] MoveTo_EffectResource m_move = default;
        [SerializeField] ExhaustActionPoint_EffectResource m_exhaustAllActionPoint = default;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            
            yield return null;
        }
    }
}