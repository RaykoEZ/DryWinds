using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Reveal_CardResource : BaseCardResourceContainer 
    {
        [SerializeField] Reveal_EffectResource m_reveal = default;
        public Reveal RevealModule => m_reveal.RevealModule;
    }
}
