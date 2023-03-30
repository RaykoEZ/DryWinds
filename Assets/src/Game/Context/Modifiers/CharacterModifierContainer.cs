using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class CharacterModifierContainer : 
        IModifierContainer<CharacterModifierProperty>
    {
        protected List<CharacterModifier> m_modifiers;
        protected List<IStatModifier<CharacterModifierProperty>> m_toRemove = 
            new List<IStatModifier<CharacterModifierProperty>>();
        protected List<CharacterModifier> m_toAdd = new List<CharacterModifier>();

        public event OnModifierExpire<CharacterModifierProperty> OnModExpire;
        public event OnModifierTrigger<CharacterModifierProperty> OnModTrigger;
        CharacterModifierProperty m_overallValue;
        public CharacterModifierProperty Result { get { return m_overallValue; } }
        public IReadOnlyList<IStatModifier<CharacterModifierProperty>> Modifiers => m_modifiers;
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
                AddModifier_Internal(newMod);
            }
            m_toAdd.Clear();
        }

        public virtual void AddModifier(IStatModifier<CharacterModifierProperty> mod) 
        {
            m_toAdd.Add(mod as CharacterModifier);
        }

        protected virtual void AddModifier_Internal(CharacterModifier mod) 
        {
            if (mod == null)
            {
                return;
            }
            mod.OnModifierExpire += OnModifierExpire;
            mod.OnTrigger += OnModifierEffectTrigger;
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

            m_modifiers.Remove(mod);
            m_overallValue = mod.Revert(m_overallValue);           
            OnModExpire?.Invoke(mod);
        }

        protected virtual void OnModifierExpire(IStatModifier<CharacterModifierProperty> mod) 
        {
            m_toRemove.Add(mod);
        }

        protected virtual void OnModifierEffectTrigger() 
        {
            UpdateModifierValue();
            OnModTrigger?.Invoke();
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
    }
}
