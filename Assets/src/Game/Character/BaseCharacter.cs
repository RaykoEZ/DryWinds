using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public delegate void OnLoadFinish();
    public delegate void OnCharacterHeal(float heal);
    public delegate void OnCharacterTakeDamage(float damage);
    public delegate void OnCharacterHitStun(float stunMod);
    public delegate void OnCharacterDefeated(Action onFinish);
    public delegate void OnCharacterInterrupt();
    public delegate void OnCharacterRetreat();
    public abstract class BaseCharacter : Interactable
    {
        [SerializeField] protected CharacterStatusManager m_statusManager = default;
        protected CharacterContextFactory m_contextFactory = new CharacterContextFactory();

        public event OnLoadFinish OnLoaded;
        public virtual CharacterStats BaseStats 
        { 
            get { return m_statusManager.BaseStats.CharacterStats; } 
        }
        public virtual CharacterStats CurrentStats 
        { 
            get { return m_statusManager.CurrentStats.CharacterStats; } 
        }
        public virtual SkillInventory BasicSkills { get { return m_statusManager.BasicSkills; } }
        public virtual SkillInventory DrawSkills { get { return m_statusManager.DrawSkills; } }

        public event OnCharacterHeal OnHealing;
        public event OnCharacterTakeDamage OnTakingDamage;
        public event OnCharacterHitStun OnHitStun;
        public event OnCharacterInterrupt OnActionInterrupt;
        public event OnCharacterDefeated OnDefeated;

        public override void Prepare() 
        {
            base.Prepare();
            m_statusManager.OnLoaded += () => { OnLoaded?.Invoke(); };
            m_statusManager.Init(this, m_contextFactory);
        }

        protected override void OnBodyHit(BodyHitResult hit)
        {
            base.OnBodyHit(hit);
            // Apply any modifiers on hit
            if(hit.Modifiers != null) 
            { 
                foreach(CharacterModifier mod in hit.Modifiers) 
                {
                    ApplyModifier(mod);
                }
            }
        }

        public override void ReturnToPool() 
        {
            m_statusManager.Shutdown();
            base.ReturnToPool();
        }

        protected override void OnTakeDamage(float damage, int partDamage = 0)
        {
            m_statusManager.TakeDamage(damage);
            OnTakingDamage?.Invoke(damage);
            OnHitStun?.Invoke(1f);
        }

        public virtual void OnHeal(float val) 
        {
            m_statusManager.Heal(val);
            OnHealing?.Invoke(val);
        }

        public virtual void OnLoseSp(float val)
        {
            m_statusManager.LoseSp(val);

        }
        public virtual void OnSPRegen()
        {
            m_statusManager.GainSp(Time.deltaTime * CurrentStats.SPRegenPerSec);
        }

        public virtual void OnGainSp(float val)
        {
            m_statusManager.GainSp(val);
        }

        public virtual void OnInterrupt() 
        {
            OnActionInterrupt?.Invoke();
        }
        protected override void OnDefeat()
        {
            OnDefeated?.Invoke(Defeat);
        }

        void Defeat() 
        {
            base.OnDefeat();
        }

        public virtual void ApplyModifier(CharacterModifier mod) 
        {
            if (mod == null)
            {
                return;
            }
            m_statusManager.AddModifier(mod);
        }
    }
}
