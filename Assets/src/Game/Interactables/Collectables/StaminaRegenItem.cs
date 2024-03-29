﻿using UnityEngine;

namespace Curry.Game
{
    public class StaminaRegenItem : EffectOverTimeItem 
    {
        [SerializeField] protected float m_healAmount = default;
        protected override void OnEffectTrigger() 
        {
            m_owner.OnHeal(m_healAmount);
        }

    }
}
