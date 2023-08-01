using System;
using UnityEngine;
namespace Curry.Explore
{   
    
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "TakeNamedCardFromHand_", menuName = "Curry/Effects/TakeNamedCardFromHand", order = 1)]
    public class TakeNamedCardFromHand_EffectResource : BaseEffectResource
    {
        
        [SerializeField] TakeNamedCardFromHand m_effect = default;
        public TakeNamedCardFromHand Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            
        }

    }
    [Serializable]
    public class TakeNamedCardFromHand : PropertyAttribute
    {
        
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            
        }
        public void ApplyEffect(ICharacter target)
        {
            
        }
    }
}