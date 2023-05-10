using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "SwapPosition_", menuName = "Curry/Effects/SwapPosition", order = 1)]
    public class SwapPosition_EffectResource : BaseEffectResource 
    {
        [SerializeField] SwapPosition m_swap = default;
        public SwapPosition SwapModule => m_swap;
        public override void Activate(GameStateContext context)
        {
            // cannot do swap by just context yet
        }
    }
}
