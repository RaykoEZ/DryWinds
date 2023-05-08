using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Heal_CardResource : BaseCardResourceContainer 
    {
        [SerializeField] Heal_EffectResource m_healing = default;
        public HealingModule Healing => m_healing.Healing;
    }
}
