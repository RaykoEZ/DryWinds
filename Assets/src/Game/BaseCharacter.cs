using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{
    public delegate void OnCharacterTakeDamage(float damage);
    public delegate void OnCharacterInterrupt();
    public abstract class BaseCharacter : Interactable
    {
        [SerializeField] protected CharacterStatsManager m_statsManager = default;

        public virtual CharacterStats BaseStats { get { return m_statsManager.BaseStats.CharacterStats; } }
        public override CollisionStats BaseCollisionStats { get { return m_statsManager.BaseStats.CollisionStats; } }

        public virtual CharacterStats CurrentStats { get { return m_statsManager.CurrentStats.CharacterStats; } }
        public virtual CollisionStats CurrentCollisionStats { get { return m_statsManager.CurrentStats.CollisionStats; } }

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

                    OnKnockback(dir, staminaRating * incomingInterable.BaseCollisionStats.Knockback);
                }
                else
                {
                    OnKnockback(dir, incomingInterable.BaseCollisionStats.Knockback);
                }
            }
        }

        public override void OnTakeDamage(float damage)
        {
            CurrentStats.Stamina = Mathf.Max(CurrentStats.Stamina - damage, 0f);
            OnTakingDamage?.Invoke(damage);
        }

        public virtual void OnInterrupt() 
        {
              OnActionInterrupt?.Invoke();
        }

    public override void OnKnockback(Vector2 direction, float knockback)
        {
            m_rigidbody.AddForce(knockback * direction, ForceMode2D.Impulse);
        }
    }
}
