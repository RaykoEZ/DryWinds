using System;
using Curry.Game;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public struct ModifierContent 
    {
        // The image to display on a status icon, like in mmos or pokemon 
        [SerializeField] public Sprite Icon;
        // Name of the modifier, e.g. a skill/item name
        [SerializeField] public string Name;
        [TextArea]
        [SerializeField] public string Description;
    }
    public abstract class TacticalModifier : IStatModifier<TacticalStats>
    {
        [SerializeField] protected ModifierContent m_content = default;
        public ModifierContent Content => m_content;
        public string Name => m_content.Name;
        public string Description => m_content.Description;
        public Sprite Icon => m_content.Icon;
        public event OnModifierExpire<TacticalStats> OnExpire;
        public event OnModifierTrigger<TacticalStats> OnTrigger;
        public TacticalStats Apply(TacticalStats baseVal) 
        {
            return Apply_Internal(baseVal);
        }
        protected void Trigger()
        {
            OnTrigger?.Invoke(this);
        }
        protected void Expire()
        {
            OnExpire?.Invoke(this);
        }
        protected abstract TacticalStats Apply_Internal(TacticalStats baseVal);
    }
}
