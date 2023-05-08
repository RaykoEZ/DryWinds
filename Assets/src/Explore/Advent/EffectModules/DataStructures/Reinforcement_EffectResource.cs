using UnityEngine;
using System;

namespace Curry.Explore
{
    [Serializable]
    public class Reinforcement_EffectResource : BaseEffectResource 
    {
        [SerializeField] Reinforcement m_reinforcement = default;
        public Reinforcement ReinforcementModule => m_reinforcement;
    }
}
