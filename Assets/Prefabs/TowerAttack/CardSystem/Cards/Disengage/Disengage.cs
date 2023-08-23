using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Disengage : CardResource, ICooldown
    {
        [SerializeField] Heal_EffectResource m_heal;
        [SerializeField] ExhaustAp_EffectResource m_exhaustActionPoint = default;
        public Disengage(Disengage effect) : base(effect)
        {
            m_heal = effect.m_heal;
            m_exhaustActionPoint = effect.m_exhaustActionPoint;
        }

        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            Action onVfxTrigger = () =>
            {
                m_exhaustActionPoint.Effect.ApplyEffect(context.ActionCount, out int numSpent);
                // heal for [base healing value] * num. of actions exhausted
                m_heal.Healing.ApplyEffect(context.Player, numSpent * m_heal.Healing.HealAmount);
            };
            var handle = user.AddVfx(m_vfx, m_vfxTimeLine);
            yield return handle?.PlayerVfx(onVfxTrigger);
            yield return new WaitForSeconds(1f);
            handle?.StopVfx();
        }
    }
}