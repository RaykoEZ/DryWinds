using Curry.Util;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    // A basic card that requires targeting a tile in close range to activate
    public class Assault : AdventCard, ITargetsPosition, ICooldown
    {
        [SerializeField] DealDamage_EffectResource m_dealDamage = default;
        public override bool ConditionsSatisfied => m_targeting.Satisfied;
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            yield return m_targeting.
                ActivateWithTargets<ICharacter, ICharacter>(user, m_dealDamage.DamageModule.ApplyEffect);
        }
    }
}
