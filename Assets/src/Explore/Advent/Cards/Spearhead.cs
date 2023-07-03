using System.Collections;
using UnityEngine;

namespace Curry.Explore
{  
    public class Spearhead : AdventCard, ITargetsPosition, ICooldown
    {
        [SerializeField] AreaAttack_EffectResource m_attack = default;
        [SerializeField] MoveTo_EffectResource m_moveTo = default;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            m_moveTo?.Effect?.ApplyEffect(user, m_targeting.Target, context.Movement, m_attack.Effect.ApplyEffect(user));
            yield return null;
        }
    }
}