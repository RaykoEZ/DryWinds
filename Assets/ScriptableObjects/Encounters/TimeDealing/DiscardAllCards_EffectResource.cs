using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "DiscardAllCards_", menuName = "Curry/Effects/DiscardAllCards", order = 1)]
    public class DiscardAllCards_EffectResource : BaseEffectResource
    {
        
        [SerializeField] DiscardAllCards m_effect = default;
        DiscardAllCards Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            Effect.Activate(context.Hand);
        }

    }
}