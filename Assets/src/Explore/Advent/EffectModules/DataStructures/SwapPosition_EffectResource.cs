using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class SwapPosition_EffectResource : BaseEffectResource 
    {
        [SerializeField] SwapPosition m_swap = default;
        public SwapPosition SwapModule => m_swap;
    }
}
