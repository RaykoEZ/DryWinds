using Curry.Util;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    // A basic card that requires targeting a tile in close range to activate
    public class Assault : AdventCard, ITargetsPosition, ICooldown
    {
        public override bool ConditionsSatisfied => m_targeting.Satisfied;
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            DealDamageTo damage = (m_resources as Assault_CardResource).DamageModule;
            yield return m_targeting.
                ActivateWithTargets<ICharacter, ICharacter>(user, damage.ApplyEffect);
        }
    }
}
