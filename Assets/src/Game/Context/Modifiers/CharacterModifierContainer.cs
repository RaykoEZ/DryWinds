using System;
using System.Collections.Generic;

namespace Curry.Game
{
    public class CharacterModifierContainer 
    {
        List<CharactertModifier> m_modifiers;

        public event OnModifierExpire OnModExpire;
        public event OnModifierChain OnModChain;
        public event OnModifierTrigger OnEffectTrigger;

        CharacterModifierProperty m_overallValue;

        public CharacterModifierProperty OverallValue { get { return m_overallValue; } }

        public CharacterModifierContainer(float initVal)
        {
            m_modifiers = new List<CharactertModifier>();
            m_overallValue = new CharacterModifierProperty(initVal);
        }

        public virtual void OnTimeElapsed(float dt, CharacterContext current) 
        {
            foreach (CharactertModifier mod in m_modifiers)
            {
                mod.OnTimeElapsed(dt, current);
            }
        }

        public virtual void Add(CharactertModifier mod) 
        {
            if (mod == null)
            {
                return;
            }
            mod.OnModifierExpire += OnModifierExpire;
            mod.OnTrigger += OnModifierEffectTrigger;
            mod.OnModifierChain += OnModChain;
            m_modifiers.Add(mod);

            m_overallValue = mod.Apply(m_overallValue);
        }

        protected virtual void OnModifierExpire(CharactertModifier mod)
        {
            if (mod == null)
            {
                return;
            }

            mod.OnModifierExpire -= OnModifierExpire;
            mod.OnTrigger -= OnModifierEffectTrigger;
            mod.OnModifierChain -= OnModChain;

            m_modifiers.Remove(mod);
            m_overallValue = mod.Revert(m_overallValue);           
            OnModExpire?.Invoke(mod);
        }

        protected virtual void OnModifierEffectTrigger() 
        {
            UpdateModifierValue();
            OnEffectTrigger?.Invoke();
        }

        protected virtual void UpdateModifierValue() 
        {
            if (m_modifiers.Count == 0)
            {
                return;
            }
            else
            {
                m_overallValue = m_modifiers[0].Value;
            }

            if (m_modifiers.Count > 1)
            {
                // Apply all modifiera to base
                for (int i = 1; i < m_modifiers.Count; ++i)
                {
                    m_overallValue = m_modifiers[i].Apply(m_overallValue);
                }
            }
        }

        protected virtual void OnModifierChain(CharactertModifier newModifier) 
        {
            OnModChain?.Invoke(newModifier);
        }
    }
}
