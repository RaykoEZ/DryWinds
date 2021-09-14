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

    public delegate void OnModifierExpire(CharactertModifier modifier);
    public delegate void OnModifierChain(CharactertModifier newModifier);
    public delegate void OnModifierTrigger();

    [Serializable]
    public abstract class CharactertModifier
    {
        // Name of the modifier, e.g. a skill/item name
        [SerializeField] protected string m_name;
        // The value of modifiers
        // duration of the modifier in seconds
        [SerializeField] protected float m_duration = default;
        [SerializeField] protected CharacterModifierProperty m_value;

        public event OnModifierExpire OnModifierExpire;
        // Used to apply additional modifiers from a modifier's effect
        public event OnModifierChain OnModifierChain;
        // Used to notify a change in modifier values
        public event OnModifierTrigger OnTrigger;

        public string Name { get { return m_name; } }
        public virtual CharacterModifierProperty Value { get { return m_value; } }

        public float Duration { get { return m_duration; } }
        public abstract ModifierOpType Type { get; }

        public CharactertModifier(string name, CharacterModifierProperty value, float duration)
        {
            m_name = name;
            m_value = value;
            m_duration = duration;
        }

        public abstract CharacterModifierProperty Apply(CharacterModifierProperty baseVal);
        public abstract CharacterModifierProperty Revert(CharacterModifierProperty baseVal);

        public virtual void OnTimeElapsed(float dt) 
        {
            m_duration -= dt;
            if(m_duration <= 0f) 
            {
                OnExpire();
            }
        }

        protected virtual void OnExpire() 
        {
            OnModifierExpire?.Invoke(this);
        }

        protected virtual void OnChain(CharactertModifier newModifier) 
        {
            OnModifierChain?.Invoke(newModifier);
        }

        protected virtual void TriggerEffect() 
        {
            OnTrigger?.Invoke();
        }
    }
}
