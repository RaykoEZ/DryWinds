using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{
    public abstract class BaseCharacter : Interactable
    {
        public abstract CharacterStats CurrentStats { get; }

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
                    staminaRating = Mathf.Min(12f, staminaRating);

                    OnKnockback(dir, staminaRating * incomingInterable.CollisionStats.Knockback);
                    OnTakeDamage(incomingInterable.CollisionStats.ContactDamage);
                }
                else
                {
                    OnKnockback(dir, incomingInterable.CollisionStats.Knockback);
                }
            }
        }

        public override void OnTakeDamage(float damage)
        {
            CurrentStats.Stamina = Mathf.Max(CurrentStats.Stamina - damage, 0f);

            if (CurrentStats.Stamina <= 0f)
            {
                OnDefeat();
            }
        }

        public override void OnKnockback(Vector2 direction, float knockback)
        {
            m_rigidbody.AddForce(knockback * direction, ForceMode2D.Impulse);
        }
    }
}
