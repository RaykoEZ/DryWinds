using System;
using System.Collections.Generic;
using Curry.Game;
using Curry.Util;
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

    // We need to display and interact with modifier in UI
    public interface IModifierDetailList
    {
        // Used to display descriptions for a character's abilities
        IReadOnlyList<ModifierContent> AbilityDetails { get; }
    }
    public abstract class TacticalModifier : IStatModifier<TacticalStats>
    {
        [SerializeField] protected BaseModifierResource m_resource = default;
        public virtual ModifierContent Content => m_resource.Content;
        public event OnModifierExpire<TacticalStats> OnExpire;
        public event OnModifierTrigger<TacticalStats> OnTrigger;
        public TacticalModifier() { }
        public TacticalModifier(TacticalModifier copy) 
        {
            m_resource = copy.m_resource;
        }
        public TacticalStats Process(TacticalStats baseVal) 
        {
            return Process_Internal(baseVal);
        }
        protected void Trigger()
        {
            OnTrigger?.Invoke(this);
        }
        protected void Expire()
        {
            OnExpire?.Invoke(this);
        }
        protected abstract TacticalStats Process_Internal(TacticalStats baseVal);
    }
}
