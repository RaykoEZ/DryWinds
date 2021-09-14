using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnCharacterTakeDamage(float damage);
    public delegate void OnCharacterInterrupt();
    public abstract class BaseCharacter : Interactable
    {
        [SerializeField] protected CharacterStatsManager m_statsManager = default;
        public virtual CharacterStats BaseStats { get { return m_statsManager.BaseStats.CharacterStats; } }
        public virtual CharacterStats CurrentStats { get { return m_statsManager.CurrentStats.CharacterStats; } }
        public override CollisionStats CurrentCollisionStats { get { return m_statsManager.CurrentStats.CharacterStats.CollisionStats; } }

        public event OnCharacterTakeDamage OnTakingDamage;
        public event OnCharacterInterrupt OnActionInterrupt;

        protected override void OnClash(Collision2D collision)
        {
            Interactable incomingInterable = collision.gameObject.GetComponent<Interactable>();
            if(incomingInterable != null) 
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

        public virtual void Init(CharacterContextFactory contextFactory) 
        {
            m_statsManager.Init(contextFactory);
        }

        public virtual void Shutdown() 
        {
            m_statsManager.Shutdown();
        }

        public override void OnTakeDamage(float damage)
        {
            m_statsManager.TakeDamage(damage);
            OnTakingDamage?.Invoke(damage);
        }

        public virtual void OnHeal(float val) 
        {
            m_statsManager.Heal(val);
        }

        public virtual void OnLoseSp(float val)
        {
            m_statsManager.LoseSp(val);

        }
        public virtual void OnGainSp(float val)
        {
            m_statsManager.GainSp(val);
        }

        public virtual void OnInterrupt() 
        {
            OnActionInterrupt?.Invoke();
        }

        public override void OnKnockback(Vector2 direction, float knockback)
        {
            m_rigidbody.AddForce(knockback * direction, ForceMode2D.Impulse);
        }

        public virtual void ApplyModifier(CharactertModifier mod) 
        {
            if (mod == null)
            {
                return;
            }

            m_statsManager.AddModifier(mod);
        }
    }
}
