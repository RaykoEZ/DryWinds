using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public delegate void OnLoadFinish();
    public delegate void OnCharacterHeal(float heal);
    public delegate void OnCharacterTakeDamage(float damage);
    public delegate void OnCharacterHitStun(float stunMod);
    public delegate void OnCharacterDefeated();
    public delegate void OnCharacterInterrupt();
    public abstract class BaseCharacter : Interactable
    {
        [SerializeField] protected CharacterStatusManager m_statusManager = default;
        public event OnLoadFinish OnLoaded;
        public virtual CharacterStats BaseStats 
        { 
            get { return m_statusManager.BaseStats.CharacterStats; } 
        }
        public virtual CharacterStats CurrentStats 
        { 
            get { return m_statusManager.CurrentStats.CharacterStats; } 
        }
        public override CollisionStats CurrentCollisionStats 
        { 
            get { return m_statusManager.CurrentStats.CharacterStats.CollisionStats; } 
        }
        public virtual SkillInventory BasicSkills { get { return m_statusManager.BasicSkills; } }
        public virtual SkillInventory DrawSkills { get { return m_statusManager.DrawSkills; } }

        public event OnCharacterHeal OnHealing;
        public event OnCharacterTakeDamage OnTakingDamage;
        public event OnCharacterHitStun OnHitStun;
        public event OnCharacterInterrupt OnActionInterrupt;
        public event OnCharacterDefeated OnDefeated;

        public virtual void Init(CharacterContextFactory contextFactory) 
        {
            m_statusManager.OnLoaded += () => { OnLoaded?.Invoke(); };
            m_statusManager.Init(this, contextFactory);
        }

        public virtual void Shutdown() 
        {
            m_statusManager.Shutdown();
        }

        public override void OnKnockback(Vector2 direction, float knockback)
        {
            base.OnKnockback(direction, knockback);
            OnHitStun?.Invoke(1f);
        }

        public override void OnTakeDamage(float damage)
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
        public override void OnDefeat(bool animate = false)
        {
            OnDefeated?.Invoke();
            base.OnDefeat(animate);
        }

        public virtual void ApplyModifier(CharactertModifier mod) 
        {
            if (mod == null)
            {
                return;
            }

            m_statusManager.AddModifier(mod);
        }
    }
}
