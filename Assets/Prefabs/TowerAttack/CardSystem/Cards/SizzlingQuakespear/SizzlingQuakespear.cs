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
            m_selfDamage?.Activate(context);
            m_gainAction?.Activate(context);
            yield return null;
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}