using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public class CharacterStatusManager : MonoBehaviour
    {
        [SerializeField] protected CharacterContext m_initStats;
        public event OnLoadFinish OnLoaded;
        protected CharacterModifierContainer m_multipliers = new CharacterModifierContainer(1f);
        protected CharacterModifierContainer m_adders = new CharacterModifierContainer(0f);
        protected CharacterModifierContainer m_specialMods = new CharacterModifierContainer(0f);
        protected IGameContextFactory<CharacterContext> m_contextFactoryRef = default;
        protected CharacterContext m_current;
        protected SkillInventory m_basicSkills = new SkillInventory();
        protected SkillInventory m_drawSkills = new SkillInventory();
        public CharacterContext BaseStats { get { return new CharacterContext(m_initStats); } }
        // Stats after modifiers
        public virtual CharacterContext PreModifierStats { get { return new CharacterContext(m_current); } }
        // Stats after modifiers
        public virtual CharacterContext CurrentStats { get { return CalculateModifiedStats(); } }
        public SkillInventory BasicSkills { get { return m_basicSkills; } protected set { m_basicSkills = value; } }
        public SkillInventory DrawSkills { get { return m_drawSkills; } protected set { m_drawSkills = value; } }
        protected bool StatusLoadFinished 
        { 
            get 
            { 
                return m_basicSkills.SkillAssetsLoaded 
                    && m_drawSkills.SkillAssetsLoaded; 
            } 
        }

        protected virtual void Start() 
        {
            m_multipliers.OnModTrigger += OnModifierTrigger;
            m_multipliers.OnModExpire += OnModifierExpire;

            m_adders.OnModTrigger += OnModifierTrigger;
            m_adders.OnModExpire += OnModifierExpire;

            m_specialMods.OnModTrigger += OnModifierTrigger;
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

        public virtual void Init(BaseCharacter user, IGameContextFactory<CharacterContext> contextFactory) 
        {
            m_contextFactoryRef = contextFactory;
            m_current = new CharacterContext(m_initStats);
            m_contextFactoryRef.UpdateContext(m_initStats);
            StartCoroutine(LoadAssets(user));
        }

        protected virtual IEnumerator LoadAssets(BaseCharacter user) 
        {
            if (!m_basicSkills.SkillAssetsLoaded) 
            {
                m_basicSkills.Init(user, m_current.BasicSkillAssetRefs, transform);
            }

            if (!m_drawSkills.SkillAssetsLoaded)
            {
                m_drawSkills.Init(user, m_current.DrawSkilllAssetRefs, transform);
            }

            yield return new WaitUntil(() => { return StatusLoadFinished; });
            OnLoaded?.Invoke();
        }

        public virtual void Shutdown() 
        {
            m_multipliers.OnModTrigger -= OnModifierTrigger;
            m_multipliers.OnModExpire -= OnModifierExpire;

            m_adders.OnModTrigger -= OnModifierTrigger;
            m_adders.OnModExpire -= OnModifierExpire;

            m_specialMods.OnModTrigger -= OnModifierTrigger;
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
            CharacterModifierProperty mult = m_multipliers.Current;
            CharacterModifierProperty add = m_adders.Current;
            return (m_current * mult) + add;
        }

        protected virtual void UpdateStats()
        {
            //Set effective stat back to base stats
            m_contextFactoryRef.UpdateContext(CalculateModifiedStats());
        }

        public virtual void AddModifier(IStatModifier<CharacterModifierProperty> mod) 
        {
            if(mod is CharacterModifier modifier) 
            {
                CharacterModifierContainer modifierContainer;
                switch (modifier.Type)
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
                modifierContainer?.AddModifier(modifier);
            }

            UpdateStats();
        }

        protected virtual void OnTimeElapsed(float dt) 
        {
            m_multipliers.OnTimeElapsed(dt);
            m_adders.OnTimeElapsed(dt);
            m_specialMods.OnTimeElapsed(dt);
        }

        protected virtual void OnModifierExpire(IStatModifier<CharacterModifierProperty> mod) 
        {
            Debug.Log($"{mod.Name}'s effect expired.");
            UpdateStats();
        }
        protected virtual void OnModifierTrigger(IStatModifier<CharacterModifierProperty> mod)
        {
            Debug.Log($"{mod.Name}'s effect triggered.");
            UpdateStats();
        }
    }
}
