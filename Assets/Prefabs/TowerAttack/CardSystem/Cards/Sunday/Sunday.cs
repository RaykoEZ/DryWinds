using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Sunday : CardResource, IConsumable
    {
        [SerializeField] GainAp_EffectResource m_actionGain = default;
        public Sunday(Sunday effect) : base(effect)
        {
            m_actionGain = effect.m_actionGain;
        }
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            yield return PlayVfx(user, user.WorldPosition);
            m_actionGain?.Activate(context);
            yield return null;
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}