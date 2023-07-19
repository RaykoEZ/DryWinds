using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Foresight : CardResource, ICooldown
    {
        [SerializeField] Reveal_EffectResource m_reveal = default;
        public Foresight(Foresight effect) : base(effect)
        {
            m_reveal = effect.m_reveal;
        }

        public override IEnumerator ActivateEffect(ICharacter target, GameStateContext context)
        {
            m_reveal?.RevealModule?.ApplyEffect(target);
            yield return new WaitForEndOfFrame();
        }
    }
}
