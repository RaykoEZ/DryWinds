using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class SizzlingQuakespear : CardResource, IConsumable
    {
        [SerializeField] DealDamage_EffectResource m_selfDamage = default;
        [SerializeField] GainAp_EffectResource m_gainAction = default;
        public SizzlingQuakespear(SizzlingQuakespear effect) : base(effect)
        {
            m_selfDamage = effect.m_selfDamage;
            m_gainAction = effect.m_gainAction;
        }
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            Action onTrigger = () =>
            {
                m_selfDamage?.Activate(context);
                m_gainAction?.Activate(context);
            };
            var vfx = user.AddVfx(m_vfx, m_vfxTimeLine);
            yield return vfx?.PlayerVfx(onTrigger);
            yield return new WaitForSeconds(0.1f);
            vfx?.StopVfx();
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}