using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "HalfHp_", menuName = "Curry/Effects/HalfHp", order = 1)]
    public class HalfHp_EffectResource : BaseEffectResource
    {
        
        [SerializeField] HalfHp m_effect = default;
        HalfHp Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            Effect.ApplyEffect(context.Player);
        }
        public void Activate(ICharacter target)
        {
            Effect.ApplyEffect(target);
        }

    }
}