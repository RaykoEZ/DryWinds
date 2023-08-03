using System;
using UnityEngine;
namespace Curry.Explore
{   
    
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "DiscardCard_", menuName = "Curry/Effects/DiscardCard", order = 1)]
    public class DiscardCard_EffectResource : BaseEffectResource
    {
        
        [SerializeField] DiscardCard m_effect = default;
        public DiscardCard Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            
        }

    }
    [Serializable]
    public class DiscardCard : PropertyAttribute
    {
        
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            
        }
        public void ApplyEffect(ICharacter target)
        {
            
        }
    }
}