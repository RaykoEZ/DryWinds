using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public delegate void OnCharacterHeal(float heal);
    public delegate void OnCharacterTakeDamage(float damage);
    public delegate void OnCharacterInterrupt();
    public abstract class BaseCharacter : Interactable
    {
        [SerializeField] protected CharacterStatusManager m_statusManager = default;
        [SerializeField] SkillHandler m_basicSkills = default;
        [SerializeField] SkillHandler m_drawSkills = default;

        public virtual CharacterStats BaseStats { get { return m_statusManager.BaseStats.CharacterStats; } }
        public virtual CharacterStats CurrentStats { get { return m_statusManager.CurrentStats.CharacterStats; } }
        public override CollisionStats CurrentCollisionStats { get { return m_statusManager.CurrentStats.CharacterStats.CollisionStats; } }

        public event OnCharacterHeal OnHealing;
        public event OnCharacterTakeDamage OnTakingDamage;
        public event OnCharacterInterrupt OnActionInterrupt;

        public override void OnKnockback(Vector2 direction, float knockback)
        {
            m_rigidbody.AddForce(knockback * direction, ForceMode2D.Impulse);
        }

        public virtual void Init(CharacterContextFactory contextFactory) 
        {
            m_statusManager.Init(contextFactory);
            m_basicSkills.Init(this);
            m_drawSkills.Init(this);
        }

        public virtual void Shutdown() 
        {
            m_statusManager.Shutdown();
        }

        public override void OnTakeDamage(float damage)
        {
            m_statusManager.TakeDamage(damage);
            OnTakingDamage?.Invoke(damage);
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
        public virtual void OnGainSp(float val)
        {
            m_statusManager.GainSp(val);
        }

        public virtual void OnInterrupt() 
        {
            OnActionInterrupt?.Invoke();
        }
        public override void OnDefeat()
        {
            base.OnDefeat();
        }

        public virtual void ApplyModifier(CharactertModifier mod) 
        {
            if (mod == null)
            {
                return;
            }

            m_statusManager.AddModifier(mod);
        }

        protected override void OnClash(Collision2D collision)
        {
            Interactable incomingInterable = collision.gameObject.GetComponent<Interactable>();
            if (incomingInterable != null)
            {
                ContactPoint2D contact = collision.GetContact(0);
                Vector2 dir = contact.normal.normalized;

                if (incomingInterable.Relations != Relations)
                {
                    float staminaRating = (CurrentStats.MaxStamina / Mathf.Max(1f, CurrentStats.Stamina));
                    staminaRating = Mathf.Min(5f, staminaRating);

                    OnKnockback(dir, staminaRating * incomingInterable.CurrentCollisionStats.Knockback);
                }
                else
                {
                    OnKnockback(dir, incomingInterable.CurrentCollisionStats.Knockback);
                }
            }
        }
    }
}
