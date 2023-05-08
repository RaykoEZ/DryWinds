using Curry.Util;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{

    public class Exile : AdventCard, ITargetsPosition, ICooldown
    {
        public override bool ConditionsSatisfied => m_targeting.Satisfied;
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            Push push = (m_resources as Push_CardResource).PushModule;
            yield return m_targeting.
                ActivateWithTargets<ICharacter, ICharacter>(user, push.ApplyEffect);
        }
    }
}
