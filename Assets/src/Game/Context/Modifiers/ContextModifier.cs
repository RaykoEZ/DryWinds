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

    public delegate void OnModifierExpire<T>(ContextModifier<T> modifier) where T:IGameContext;
    public delegate void OnModifierChainReaction<T>(ContextModifier<T> modifier, bool isBaseModifier = true) where T : IGameContext;
    public delegate void OnModifierTrigger();

    [Serializable]
    public abstract class ContextModifier<T> where T : IGameContext
    {
        // Name of the modifier, e.g. a skill/item name
        [SerializeField] protected string m_name;
        // The value of modifiers
        [SerializeField] protected T m_value;
        // duration of the modifier in seconds
        [SerializeField] protected float m_duration = default;
        
        public event OnModifierExpire<T> OnModifierExpire;
        public event OnModifierChainReaction<T> OnModifierChain;
        // Used to notify a change in modifier values
        public event OnModifierTrigger OnTrigger;

        public string Name { get { return m_name; } }
        public T Value { get { return m_value; } }

        public float Duration { get { return m_duration; } }
        public abstract ModifierOpType Type { get; }

        public ContextModifier(string name, T value, float duration)
        {
            m_name = name;
            m_value = value;
            m_duration = duration;
        }

        public abstract T Apply(T baseVal);
        public abstract T Revert(T baseVal);

        public virtual void OnTimeElapsed(float dt, CharacterContext current) 
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

        protected virtual void TriggerEffect(CharacterContext current) 
        {
            OnTrigger?.Invoke();
        }
    }
}
