﻿using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class Healing : AdventCard, IConsumable
    {
        [SerializeField] Heal_EffectResource m_healing = default;
        // Card Effect
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            m_healing?.Healing?.ApplyEffect(user, user);
            yield return null;
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}
