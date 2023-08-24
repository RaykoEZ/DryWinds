using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "ChooseLoot_", menuName = "Curry/Effects/ChooseLoot", order = 1)]
    public class ChooseLoot_EffectResource : BaseEffectResource
    {        
        [SerializeField] ChooseLoot m_effect = default;
        public override void Activate(GameStateContext context)
        {
            m_effect.ApplyEffect(context.Deck, context.LootManager);
        }
    }
}