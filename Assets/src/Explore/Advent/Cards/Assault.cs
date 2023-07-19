using Curry.Util;
using System;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    // A basic card that requires targeting a tile in close range to activate
    public class Assault : CardResource, ITargetsPosition, ICooldown
    {
        [SerializeField] DealDamage_EffectResource m_dealDamage = default;

        public Assault(Assault effect) : base(effect)
        {
            m_dealDamage = effect.m_dealDamage;
        }
        public override bool ConditionsSatisfied => m_targeting.Satisfied;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            yield return m_targeting.
                ActivateWithTargets<ICharacter, ICharacter>(user, m_dealDamage.DamageModule.ApplyEffect);
        }
    }
}
