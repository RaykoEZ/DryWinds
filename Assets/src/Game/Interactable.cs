using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{
    // Player: does not damage player
    // Hostile: damages player
    public enum ObjectRelations 
    {
        Player,
        Hostile
    }

    // A basic script for a collidable object 
    public class Interactable : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D m_rigidbody = default;
        [SerializeField] protected ObjectRelations m_relations = default;
        CollisionStats m_defaultCollisionStats = new CollisionStats(0f, 5f);

        public virtual CollisionStats CollisionStats { get { return m_defaultCollisionStats; } }
        public ObjectRelations Relations { get { return m_relations; } }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            OnClash(collision);         
        }

        protected virtual void OnClash(Collision2D collision)
        {
            Interactable incomingInterable = collision.gameObject.GetComponent<Interactable>();
            if (incomingInterable != null)
            {
                ContactPoint2D contact = collision.GetContact(0);
                Vector2 dir = contact.normal.normalized;
                float vFactor = InteractionUtil.ScaleFactior(
                    incomingInterable.m_rigidbody.velocity.sqrMagnitude,
                    m_rigidbody.velocity.sqrMagnitude,
                    0.2f,
                    1.5f);

                OnKnockback(dir, vFactor * incomingInterable.CollisionStats.Knockback);
            }
        }

        public virtual void OnKnockback(Vector2 direction, float knockback)
        {
            m_rigidbody.AddForce(knockback * direction, ForceMode2D.Impulse);
        }


        protected virtual void OnTakeDamage(float damage) 
        {          
        }

        public virtual void OnDefeat() 
        {      
        }
    }
}
