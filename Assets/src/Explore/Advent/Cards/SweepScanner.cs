﻿using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class SweepScanner : AdventCard, ICooldown
    {
        [SerializeField] Reveal_EffectResource m_reveal = default;
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            m_reveal?.RevealModule?.ApplyEffect(user, user);
            yield return new WaitForEndOfFrame();
        }
    }
}
