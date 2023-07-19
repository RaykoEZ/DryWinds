using Curry.Util;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class Exile : CardResource, ITargetsPosition, ICooldown
    {
        [SerializeField] Push_EffectResource m_push = default;
        public Exile(Exile effect) : base(effect)
        {
            m_push = effect.m_push;
        }
        public override bool ConditionsSatisfied => m_targeting.Satisfied;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            yield return m_targeting.
                ActivateWithTargets<ICharacter, ICharacter>(user, m_push.PushModule.ApplyEffect);
        }
    }
}
