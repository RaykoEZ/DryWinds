using UnityEngine;
using System;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Reinforcement_", menuName = "Curry/Effects/Reinforcement", order = 1)]
    public class Reinforcement_EffectResource : BaseEffectResource 
    {
        [SerializeField] Reinforcement m_reinforcement = default;
        [SerializeField] ReinforcementList m_targets = default;
        public Reinforcement ReinforcementModule => m_reinforcement;
        public ReinforcementList TargetPool => m_targets;
        public override void Activate(GameStateContext context)
        {
            // cannot spawn on player
        }
    }
}
