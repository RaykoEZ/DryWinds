using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "DiscardCard_", menuName = "Curry/Effects/DiscardCard", order = 1)]
    public class DiscardCard_Random_EffectResource : BaseEffectResource
    {      
        [SerializeField] DiscardCard_Random m_effect = default;
        DiscardCard_Random Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            Effect.ApplyEffect(context.Hand);
        }
        public void ApplyEffect(GameStateContext context, int numToDiscard)
        {
            Effect.ApplyEffect(context.Hand, numToDiscard);
        }
    }
}