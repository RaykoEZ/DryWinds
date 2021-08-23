using System.Collections.Generic;

namespace Curry.Game
{
    public class CharacterModifierContainer 
    {
        List<ContextModifier<CharacterContext>> m_modifiers;

        public event OnModifierExpire<CharacterContext> OnModExpire;
        public event OnModifierValueChange OnValueChange;

        CharacterContext m_overallValue;

        public CharacterContext OverallValue { get { return m_overallValue; } }

        public CharacterModifierContainer() 
        {
            m_modifiers = new List<ContextModifier<CharacterContext>>();
        }

        public virtual void OnTimeElapsed(float dt) 
        {
            foreach (ContextModifier<CharacterContext> mod in m_modifiers)
            {
                mod.OnTimeElapsed(dt);
            }
        }

        public virtual void Add(ContextModifier<CharacterContext> mod) 
        {
            if (mod == null)
            {
                return;
            }
            mod.OnModifierExpire += OnModifierExpire;
            mod.OnValueChange += OnModifierValueChanged;
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
            mod.OnValueChange -= OnModifierValueChanged;

            m_modifiers.Remove(mod);

            if(m_overallValue != null) 
            {
                m_overallValue = mod.Revert(m_overallValue);
            }

            OnModExpire?.Invoke(mod);
        }

        protected virtual void OnModifierValueChanged() 
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

            OnValueChange?.Invoke();
        }
    }
}
