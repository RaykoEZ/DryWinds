using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "GainAP_", menuName = "Curry/Effects/Gain AP", order = 1)]
    public class GainAp_EffectResource : BaseEffectResource 
    {
        [SerializeField] MovementPointChange m_gain = default;
        public MovementPointChange ApGain => m_gain;
        public override void Activate(GameStateContext context)
        {
            m_gain?.ApplyEffect(context.ActionCount);
        }
    }
}
