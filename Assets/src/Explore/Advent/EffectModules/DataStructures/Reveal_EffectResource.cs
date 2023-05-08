using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Reveal_EffectResource : BaseEffectResource
    {
        [SerializeField] Reveal m_reveal = default;
        public Reveal RevealModule => m_reveal;
    }
}
