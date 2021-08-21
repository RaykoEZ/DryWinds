using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnModifierExpire<T>(ContextModifier<T> modifier) where T:IGameContext;
    public class ContextModifier<T> where T : IGameContext
    {
        public event OnModifierExpire<T> OnModifierExpire;
        // The value of modifiers
        protected T m_value;
        // duration of the modifier in seconds
        protected float m_duration = default;

        public float Duration { get { return m_duration; } }

        public T Value { get { return m_value; } }

        public ContextModifier(T value, float duration)
        {
            m_value = value;
            m_duration = duration;
        }

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
    }
}
