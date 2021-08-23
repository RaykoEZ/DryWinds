﻿using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnCharacterStatsUpdate(CharacterContext c);
    public class CharacterStatsManager : MonoBehaviour
    {
        [SerializeField] protected CharacterContext m_baseStats;

        protected Dictionary<ModifierOpType, CharacterModifierContainer> m_baseMods =
            new Dictionary<ModifierOpType, CharacterModifierContainer>();

        protected Dictionary<ModifierOpType, CharacterModifierContainer> m_globalMods =
            new Dictionary<ModifierOpType, CharacterModifierContainer>();

        protected IGameContextFactory<CharacterContext> m_contextFactoryRef = default;
        protected CharacterContext m_currentStats;

        public virtual CharacterContext BaseStats { get { return new CharacterContext(m_baseStats); } }
        public virtual CharacterContext CurrentStats { get { return m_currentStats; } }

        protected virtual void Update() 
        {
            if (CurrentStats.IsDirty)
            {
                m_contextFactoryRef.UpdateContext(CurrentStats);
            }

            OnTimeElapsed(Time.deltaTime);
        }

        public virtual void Init(IGameContextFactory<CharacterContext> contextFactory) 
        {
            m_contextFactoryRef = contextFactory;
            m_contextFactoryRef.OnUpdate += OnContextUpdated;
            m_contextFactoryRef.UpdateContext(BaseStats);
        }

        public virtual void Shutdown() 
        {
            m_contextFactoryRef.OnUpdate -= OnContextUpdated;
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
                m_baseMods[modifier.Type].OnValueChange += UpdateStats;
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
            if (m_baseMods.TryGetValue(ModifierOpType.Multiply, out CharacterModifierContainer baseMult) && 
                baseMult.OverallValue != null) 
            {
                m_currentStats *= baseMult.OverallValue;
            }
            if (m_baseMods.TryGetValue(ModifierOpType.Add, out CharacterModifierContainer baseAdder) 
                && baseAdder.OverallValue != null)
            {
                m_currentStats += baseAdder.OverallValue;
            }
            if (m_globalMods.TryGetValue(ModifierOpType.Add, out CharacterModifierContainer globalMult) && 
                globalMult.OverallValue != null)
            {
                m_currentStats *= globalMult.OverallValue;
            }
            if (m_globalMods.TryGetValue(ModifierOpType.Add, out CharacterModifierContainer globalAdder) && 
                globalAdder.OverallValue != null)
            {
                m_currentStats += globalAdder.OverallValue;
            }
        }

        protected virtual void OnModifierExpire(ContextModifier<CharacterContext> mod) 
        {
            Debug.Log($"{mod.Name}'s effect expired.");
            UpdateStats();
        }

        protected virtual void OnContextUpdated(CharacterContext c) 
        {
            m_currentStats = c;
            UpdateStats();
        }
    }
}
