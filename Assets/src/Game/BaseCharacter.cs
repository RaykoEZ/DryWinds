using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public abstract class BaseCharacter : Interactable
    {
        public abstract CharacterStats CurrentStats { get; }

        protected override void OnClash(Collision2D collision)
        {
            Interactable incomingInterable = collision.gameObject.GetComponent<Interactable>();

            if (incomingInterable.Relations == ObjectRelations.Hostile)
            {
                OnTakeDamage(incomingInterable.CollisionStats.ContactDamage);
            }

            base.OnClash(collision);
        }

        protected override void OnTakeDamage(float damage)
        {
            CurrentStats.Stamina -= damage;

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
