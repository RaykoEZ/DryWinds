using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Reveal_", menuName = "Curry/Effects/Reveal", order = 1)]
    public class Reveal_EffectResource : BaseEffectResource
    {
        [SerializeField] Reveal m_reveal = default;
        public Reveal RevealModule => m_reveal;

        public override void Activate(GameStateContext context)
        {
            // need to implement later
        }
    }
}
