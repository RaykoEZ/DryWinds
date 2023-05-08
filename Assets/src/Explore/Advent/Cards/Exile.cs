using Curry.Util;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{

    public class Exile : AdventCard, ITargetsPosition, ICooldown
    {
        [SerializeField] Push_EffectResource m_push = default;
        public override bool ConditionsSatisfied => m_targeting.Satisfied;
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            yield return m_targeting.
                ActivateWithTargets<ICharacter, ICharacter>(user, m_push.PushModule.ApplyEffect);
        }
    }
}
