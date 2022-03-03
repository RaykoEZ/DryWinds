using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class CharacterModifierContainer 
    {
        protected List<CharacterModifier> m_modifiers;
        protected List<CharacterModifier> m_toRemove = new List<CharacterModifier>();
        protected List<CharacterModifier> m_toAdd = new List<CharacterModifier>();

        public event OnModifierExpire OnModExpire;
        public event OnModifierChain OnModChain;
        public event OnModifierTrigger OnEffectTrigger;

        CharacterModifierProperty m_overallValue;

        public CharacterModifierProperty OverallValue { get { return m_overallValue; } }

        public CharacterModifierContainer(float initVal)
        {
            m_modifiers = new List<CharacterModifier>();
            m_overallValue = new CharacterModifierProperty(initVal);
        }

        public virtual void OnTimeElapsed(float dt) 
        {
            foreach (CharacterModifier mod in m_modifiers)
            {
                mod.OnTimeElapsed(dt);
            }

            // Clear all expired mods this frame
            foreach (CharacterModifier expired in m_toRemove) 
            {
                RemoveExpiredModifier(expired);
            }
            m_toRemove.Clear();

            //Add all new modifiers
            foreach (CharacterModifier newMod in m_toAdd)
            {
                AddModifier(newMod);
            }
            m_toAdd.Clear();
        }

        public virtual void Add(CharacterModifier mod) 
        {
            m_toAdd.Add(mod);
        }

        protected virtual void AddModifier(CharacterModifier mod) 
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

        protected virtual void RemoveExpiredModifier(CharacterModifier mod)
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

        protected virtual void OnModifierExpire(CharacterModifier mod) 
        {
            m_toRemove.Add(mod);
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

        protected virtual void OnModifierChain(CharacterModifier newModifier) 
        {
            OnModChain?.Invoke(newModifier);
        }
    }
}
