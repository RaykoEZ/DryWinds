using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "SetHp_", menuName = "Curry/Effects/SetHp", order = 1)]
    public class SetHp_EffectResource : BaseEffectResource
    {     
        [SerializeField] SetHp m_effect = default;
        public SetHp Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            Effect.ApplyEffect(context.Player);
        }
        public void Activate(ICharacter target, int setTo)
        {
            Effect.ApplyEffect(target, setTo);
        }
    }
}