﻿using System;
using System.Collections.Generic;

namespace Curry.Game
{
    public class CharacterModifierContainer 
    {
        List<ContextModifier<CharacterContext>> m_modifiers;

        public event OnModifierExpire<CharacterContext> OnModExpire;
        public event OnModifierChain<CharacterContext> OnModChain;
        public event OnModifierTrigger OnEffectTrigger;

        CharacterContext m_overallValue;

        public CharacterContext OverallValue { get { return m_overallValue; } }

        public CharacterModifierContainer() 
        {
            m_modifiers = new List<ContextModifier<CharacterContext>>();
        }

        public virtual void OnTimeElapsed(float dt, CharacterContext current) 
        {
            foreach (ContextModifier<CharacterContext> mod in m_modifiers)
            {
                mod.OnTimeElapsed(dt, current);
            }
        }

        public virtual void Add(ContextModifier<CharacterContext> mod) 
        {
            if (mod == null)
            {
                return;
            }
            mod.OnModifierExpire += OnModifierExpire;
            mod.OnTrigger += OnModifierEffectTrigger;
            mod.OnModifierChain += OnModChain;
            m_modifiers.Add(mod);

            if (m_overallValue == null) 
            {
                m_overallValue = mod.Value;
            }
            else 
            {
                m_overallValue = mod.Apply(m_overallValue);
            }
        }

        protected virtual void OnModifierExpire(ContextModifier<CharacterContext> mod)
        {
            if (mod == null)
            {
                return;
            }

            mod.OnModifierExpire -= OnModifierExpire;
            mod.OnTrigger -= OnModifierEffectTrigger;
            mod.OnModifierChain -= OnModChain;

            m_modifiers.Remove(mod);

            if(m_overallValue != null) 
            {
                m_overallValue = mod.Revert(m_overallValue);
            }

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

        protected virtual void OnModifierChain(ContextModifier<CharacterContext> newModifier, bool isBaseModifier = true) 
        {
            OnModChain?.Invoke(newModifier, isBaseModifier);
        }
    }
}
