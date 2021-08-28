using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnCharacterStatsUpdate(CharacterContext c);
    public class CharacterStatsManager : MonoBehaviour
    {
        [SerializeField] protected CharacterContext m_initStats;

        protected CharacterModifierContainer m_multipliers = new CharacterModifierContainer(1f);
        protected CharacterModifierContainer m_adders = new CharacterModifierContainer(0f);

        protected CharacterModifierContainer m_specialMods = new CharacterModifierContainer(0f);
        protected IGameContextFactory<CharacterContext> m_contextFactoryRef = default;

        protected CharacterContext m_current;
        public CharacterContext BaseStats { get { return new CharacterContext(m_initStats); } }
        // Stats after modifiers
        public virtual CharacterContext PreModifierStats { get { return new CharacterContext(m_current); } }
        // Stats after modifiers
        public virtual CharacterContext CurrentStats { get { return CalculateModifiedStats(); } }

        protected virtual void Start() 
        {
            m_multipliers.OnEffectTrigger += UpdateStats;
            m_multipliers.OnModChain += AddModifier;
            m_multipliers.OnModExpire += OnModifierExpire;

            m_adders.OnEffectTrigger += UpdateStats;
            m_adders.OnModChain += AddModifier;
            m_adders.OnModExpire += OnModifierExpire;

            m_specialMods.OnEffectTrigger += UpdateStats;
            m_specialMods.OnModChain += AddModifier;
            m_specialMods.OnModExpire += OnModifierExpire;
        }

        protected virtual void Update() 
        {
            //  if stats modified, update context for everyone
            if (m_current.IsDirty)
            {
                UpdateStats();
            }

            OnTimeElapsed(Time.deltaTime);
        }

        public virtual void Init(IGameContextFactory<CharacterContext> contextFactory) 
        {
            m_contextFactoryRef = contextFactory;
            m_current = new CharacterContext(m_initStats);
            m_contextFactoryRef.UpdateContext(m_initStats);
        }

        public virtual void Shutdown() 
        {
            m_multipliers.OnEffectTrigger -= UpdateStats;
            m_multipliers.OnModChain -= AddModifier;
            m_multipliers.OnModExpire -= OnModifierExpire;

            m_adders.OnEffectTrigger -= UpdateStats;
            m_adders.OnModChain -= AddModifier;
            m_adders.OnModExpire -= OnModifierExpire;

            m_specialMods.OnEffectTrigger -= UpdateStats;
            m_specialMods.OnModChain -= AddModifier;
            m_specialMods.OnModExpire -= OnModifierExpire;
        }
        
        public virtual void TakeDamage(float val) 
        {
            m_current.CharacterStats.Stamina -= val;
        }
        public virtual void Heal(float val) 
        {
            m_current.CharacterStats.Stamina += val;
        }
        public virtual void LoseSp(float val) 
        {
            m_current.CharacterStats.SP -= val;
        }
        public virtual void GainSp(float val)
        {
            m_current.CharacterStats.SP += val;
        }

        protected virtual CharacterContext CalculateModifiedStats()
        {
            CharacterModifierProperty mult = m_multipliers.OverallValue;
            CharacterModifierProperty add = m_adders.OverallValue;
            return (m_current * mult) + add;
        }

        protected virtual void UpdateStats()
        {
            //Set effective stat back to base stats
            m_contextFactoryRef.UpdateContext(CalculateModifiedStats());
        }

        public virtual void AddModifier(CharactertModifier mod) 
        {
            CharacterModifierContainer modifierContainer;
            switch (mod.Type)
            {
                case ModifierOpType.Add:
                    modifierContainer = m_adders;
                    break;
                case ModifierOpType.Multiply:
                    modifierContainer = m_multipliers;
                    break;
                case ModifierOpType.Special:
                    modifierContainer = m_specialMods;
                    break;
                default:
                    return;
            }

            modifierContainer?.Add(mod);
            UpdateStats();
        }

        protected virtual void OnTimeElapsed(float dt) 
        {
            m_multipliers.OnTimeElapsed(dt);
            m_adders.OnTimeElapsed(dt);
            m_specialMods.OnTimeElapsed(dt);
        }

        protected virtual void OnModifierExpire(CharactertModifier mod) 
        {
            Debug.Log($"{mod.Name}'s effect expired.");
            UpdateStats();
        }
    }
}
