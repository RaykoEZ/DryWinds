﻿using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "MultiplyCountdownRate_", menuName = "Curry/Effects/MultiplyCountdownRate", order = 1)]
    public class MultiplyCountdownRate_EffectResource : BaseEffectResource
    {   
        [SerializeField] MultiplyCountdownRate m_effect = default;
        MultiplyCountdownRate Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            Effect.ApplyEffect(context.Time);
        }
        public void Activate(GameStateContext context, float multiplier)
        {
            Effect.ApplyEffect(context.Time, multiplier);
        }
    }
}