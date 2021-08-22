using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class CharacterStatsManager : MonoBehaviour
    {
        Dictionary<ModifierOpType, CharacterModifierContainer> m_baseMods =
            new Dictionary<ModifierOpType, CharacterModifierContainer>();

        Dictionary<ModifierOpType, CharacterModifierContainer> m_globalMods =
            new Dictionary<ModifierOpType, CharacterModifierContainer>();

        CharacterContext m_baseStats;
        CharacterContext m_effectiveStats;

        public virtual CharacterContext BaseStats { get { return m_baseStats; } }
        public virtual CharacterContext EffectiveStats { get { return m_effectiveStats; } }

        protected virtual void Update() 
        {
            OnTimeElapsed(Time.deltaTime);
        }

        public virtual void Init(IGameContextFactory<CharacterContext> contextFactory) 
        {
            contextFactory.OnUpdate += OnContextUpdated;
        }

        public virtual void Shutdown(IGameContextFactory<CharacterContext> contextFactory) 
        {
            contextFactory.OnUpdate -= OnContextUpdated;
        }

        // Add a modifier on the base stats
        public virtual void AddBaseModifier(ContextModifier<CharacterContext> modifier) 
        { 
            if(modifier == null) 
            { 
                return; 
            }

            if (!m_baseMods.ContainsKey(modifier.Type)) 
            {
                m_baseMods.Add(modifier.Type, new CharacterModifierContainer());
                m_baseMods[modifier.Type].OnModExpire += OnModifierExpire;
            }

            m_baseMods[modifier.Type].Add(modifier);
            UpdateStats();
        }

        // Add a modifier that will operate on the modifier base context.
        public virtual void AddGlobalModifier(ContextModifier<CharacterContext> modifier)
        {
            if (modifier == null)
            {
                return;
            }

            if (!m_globalMods.ContainsKey(modifier.Type))
            {
                m_globalMods.Add(modifier.Type, new CharacterModifierContainer());
                m_globalMods[modifier.Type].OnModExpire += OnModifierExpire;
            }

            m_globalMods[modifier.Type].Add(modifier);
            UpdateStats();
        }

        protected virtual void OnTimeElapsed(float dt) 
        { 
            foreach(KeyValuePair<ModifierOpType, CharacterModifierContainer> kvp in m_baseMods) 
            {
                kvp.Value.OnTimeElapsed(dt);
            }


        }

        protected virtual void UpdateStats() 
        {
            //to do

        }

        protected virtual void OnModifierExpire(ContextModifier<CharacterContext> mod) 
        {
            Debug.Log($"{mod.Name}'s effect expired.");
            UpdateStats();
        }

        protected virtual void OnContextUpdated(CharacterContext c) 
        {
            m_baseStats = c;
            UpdateStats();
        }
    }
}
