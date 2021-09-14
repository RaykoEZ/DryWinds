using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{
    // Player: does not damage player
    // Enemy: damages player
    [Flags]
    public enum ObjectRelations 
    {
        None,
        Ally = 1,
        Enemy = 1 << 1
    }

    // A basic script for a collidable object 
    public class Interactable : MonoBehaviour, IPoolable
    {
        [SerializeField] protected Rigidbody2D m_rigidbody = default;
        [SerializeField] protected Collider2D m_hurtBox = default;
        [SerializeField] protected ObjectRelations m_relations = default;
        CollisionStats m_defaultCollisionStats = new CollisionStats(0f, 5f);
        public virtual IObjectPool Origin { get; set; }
        public Rigidbody2D RigidBody { get { return m_rigidbody; } }
        public Collider2D HurtBox { get { return m_hurtBox; } }
        public ObjectRelations Relations { get { return m_relations; } }
        public virtual CollisionStats CurrentCollisionStats { get { return m_defaultCollisionStats; } }

        public virtual void Prepare() 
        { }
        public void ReturnToPool()
        {
            Origin.ReturnToPool(this);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.otherCollider == m_hurtBox) 
            {
                OnClash(collision);
            }
        }

        protected virtual void OnClash(Collision2D collision)
        {
            Interactable incomingInterable = collision.gameObject.GetComponent<Interactable>();
            if (incomingInterable != null)
            {
                ContactPoint2D contact = collision.GetContact(0);
                Vector2 dir = contact.normal.normalized;
                float vFactor = GameUtil.ScaleFactior(
                    incomingInterable.m_rigidbody.velocity.sqrMagnitude,
                    m_rigidbody.velocity.sqrMagnitude,
                    0.2f,
                    1.5f);

                OnKnockback(dir, vFactor * incomingInterable.CurrentCollisionStats.Knockback);
            }
        }

        public virtual void OnKnockback(Vector2 direction, float knockback)
        {
            m_rigidbody.AddForce(knockback * direction, ForceMode2D.Impulse);
        }


        public virtual void OnTakeDamage(float damage) 
        {          
        }

        public virtual void OnDefeat() 
        {
            if (Origin != null) 
            {
                ReturnToPool();
            }
            else 
            {
                Destroy(gameObject);
            }
        }

        
    }
}
