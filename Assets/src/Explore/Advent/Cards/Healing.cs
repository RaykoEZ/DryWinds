using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Healing : CardResource, IConsumable
    {
        [SerializeField] Heal_EffectResource m_healing = default;

        public Healing(Healing effect) : base(effect)
        {
            m_healing = effect.m_healing;
        }

        // Card Effect
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            m_healing?.Healing?.ApplyEffect(user);
            yield return null;
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}
