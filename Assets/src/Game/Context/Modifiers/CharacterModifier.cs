using Curry.Explore;
using System;
using UnityEngine;

namespace Curry.Game
{
    public enum ModifierOpType
    {
        Add,
        Multiply,
        // For triggerng skill effects
        Special
    }

    public delegate void OnModifierExpire<T>(IStatModifier<T> modifier);
    public delegate void OnModifierChain<T>(IStatModifier<T> newModifier);
    public delegate void OnModifierTrigger<T>(IStatModifier<T> modifier);

    [Serializable]
    public abstract class CharacterModifier : IStatModifier<CharacterModifierProperty>
    {
        // Name of the modifier, e.g. a skill/item name
        [SerializeField] protected string m_name;
        // The value of modifiers
        // duration of the modifier in seconds
        [SerializeField] protected float m_duration = default;
        [SerializeField] protected CharacterModifierProperty m_value;

        public event OnModifierExpire<CharacterModifierProperty> OnExpire;
        // Used to notify when modifier applied
        public event OnModifierTrigger<CharacterModifierProperty> OnTrigger;

        public string Name { get { return m_name; } }
        public virtual CharacterModifierProperty Value { get { return m_value; } }

        public float Duration { get { return m_duration; } }
        public abstract ModifierOpType Type { get; }

        public virtual ModifierContent Content => new ModifierContent { Description = "", Icon = null, Name = m_name};

        public CharacterModifier(string name, CharacterModifierProperty value, float duration)
        {
            m_name = name;
            m_value = value;
            m_duration = duration;
        }

        public abstract CharacterModifierProperty Process(CharacterModifierProperty baseVal);
        public abstract CharacterModifierProperty Revert(CharacterModifierProperty baseVal);

        public virtual void OnTimeElapsed(float dt) 
        {
            m_duration -= dt;
            if(m_duration <= 0f) 
            {
                OnModExpire();
            }
        }

        protected virtual void OnModExpire() 
        {
            OnExpire?.Invoke(this);
        }

        protected virtual void TriggerEffect() 
        {
            OnTrigger?.Invoke(this);
        }
    }
}
