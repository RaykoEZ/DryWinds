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
        [SerializeField] protected BodyManager m_bodyManager = default;
        [SerializeField] protected Rigidbody2D m_rigidbody = default;
        [SerializeField] protected CharacterStatusManager m_statusManager = default;
        protected CharacterContextFactory m_contextFactory = new CharacterContextFactory();

        public event OnLoadFinish OnLoaded;
        public Rigidbody2D RigidBody { get { return m_rigidbody; } }

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
            m_bodyManager.Init();
            m_bodyManager.OnBodyPartHit += OnBodyHit;
            m_statusManager.OnLoaded += () => { OnLoaded?.Invoke(); };
            m_statusManager.Init(this, m_contextFactory);
        }

        protected virtual void OnBodyHit(BodyHitResult hit)
        {
            OnTakeDamage(hit.Damage, hit.PartDamage);
            if (hit.PartBreak)
            {
                OnBodyPartBreak(hit.BodyPart);
            }
            if (hit.WeakpointBreak)
            {
                OnWeakpointBreak(hit.BodyPart);
            }
            
            // Apply any modifiers on hit
            if (hit.Modifiers != null) 
            { 
                foreach(CharacterModifier mod in hit.Modifiers) 
                {
                    ApplyModifier(mod);
                }
            }
        }

        protected virtual void OnBodyPartBreak(BodyPart part)
        {
        }

        protected virtual void OnWeakpointBreak(BodyPart part)
        {
        }

        public override void ReturnToPool() 
        {
            m_bodyManager.Shutdown();
            m_statusManager.Shutdown();
            base.ReturnToPool();
        }

        public override void OnKnockback(Vector2 source, float knockback = 1f)
        {
            Vector2 diff = RigidBody.position - source;
            m_rigidbody.AddForce(knockback * diff.normalized, ForceMode2D.Impulse);
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
