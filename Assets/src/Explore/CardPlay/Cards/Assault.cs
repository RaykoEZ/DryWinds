using System;
using System.Collections;
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
        public override bool ConditionsSatisfied => m_targeting != null && m_targeting.TryGetCurrentTarget<IEnemy>(out _) && m_targeting.Satisfied;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            if (m_targeting.TryGetCurrentTarget(out ICharacter result))
            {
                var vfx = result.AddVfx(m_vfx, m_vfxTimeLine);
                yield return vfx?.PlayerVfx(() =>
                {
                    m_dealDamage.DamageModule.ApplyEffect(result);
                });
                vfx?.StopVfx();
            }
            yield return null;
        }
    }
}
