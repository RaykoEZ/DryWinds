using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Heal_EffectResource : BaseEffectResource
    {
        [SerializeField] HealingModule m_healing = default;
        public HealingModule Healing => m_healing;
    }
}
