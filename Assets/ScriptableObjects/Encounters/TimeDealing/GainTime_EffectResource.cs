using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "GainTime_", menuName = "Curry/Effects/GainTime", order = 1)]
    public class GainTime_EffectResource : BaseEffectResource
    {     
        [SerializeField] GainTime m_effect = default;
        GainTime Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            Effect.ApplyEffect(context.Time);
        }
        public void Activate(GameStateContext context, int toGain) 
        {
            Effect.ApplyEffect(context.Time, toGain);
        }
    }
}