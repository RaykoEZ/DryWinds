using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "Discard_ByName_", menuName = "Curry/Effects/Discard_ByName", order = 1)]
    public class Discard_ByName_EffectResource : BaseEffectResource
    {    
        [SerializeField] Discard_ByName m_effect = default;
        Discard_ByName Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            Effect.Activate(context.Hand);
        }
        public void Activate(GameStateContext context, string cardName)
        {
            Effect.Activate(context.Hand, cardName);
        }

    }
}