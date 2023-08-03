using System;
using UnityEngine;
namespace Curry.Explore
{   
    
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "TriggerCooldownInHand_", menuName = "Curry/Effects/TriggerCooldownInHand", order = 1)]
    public class TriggerCooldownInHand_EffectResource : BaseEffectResource
    {
        
        [SerializeField] TriggerCooldownInHand m_effect = default;
        public TriggerCooldownInHand Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            
        }

    }
    [Serializable]
    public class TriggerCooldownInHand : PropertyAttribute
    {
        
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            
        }
        public void ApplyEffect(ICharacter target)
        {
            
        }
    }
}