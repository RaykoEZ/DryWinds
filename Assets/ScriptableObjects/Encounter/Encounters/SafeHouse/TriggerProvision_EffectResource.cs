using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "TriggerProvision_", menuName = "Curry/Effects/TriggerProvision", order = 1)]
    public class TriggerProvision_EffectResource : BaseEffectResource
    {       
        [SerializeField] TriggerProvision m_effect = default;
        public TriggerProvision Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            
        }

    }
}