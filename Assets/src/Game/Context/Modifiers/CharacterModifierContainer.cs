using System.Collections.Generic;

namespace Curry.Game
{
    public class CharacterModifierContainer 
    {
        List<ContextModifier<CharacterContext>> m_modifiers;
        List<ContextModifier<CharacterContext>> m_toRemove;

        public event OnModifierExpire<CharacterContext> OnModExpire;
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

            m_toRemove.Add(mod);

            mod.OnModifierExpire -= OnModifierExpire;
            m_modifiers.Remove(mod);

            if(m_overallValue != null) 
            {
                m_overallValue = mod.Revert(m_overallValue);
            }

            OnModExpire?.Invoke(mod);
        }
    }
}
