using System;
using System.Collections;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{
    // A basic script for a collidable object 
    public class Interactable : MonoBehaviour, IPoolable, IClashable
    {
        [SerializeField] protected BodyManager m_bodyManager = default;
        [SerializeField] protected Rigidbody2D m_rigidbody = default;
        CollisionStats m_defaultCollisionStats = new CollisionStats(0f, 5f);
        public virtual IObjectPool Origin { get; set; }
        public Rigidbody2D RigidBody { get { return m_rigidbody; } }
        public virtual CollisionStats CollisionData { get { return m_defaultCollisionStats; } }

        void Awake() 
        { 
            // If this object isn't in a pool, init here
            if(Origin == null) 
            {
                Prepare();
            }
        }

        public virtual void Prepare() 
        {
            m_bodyManager.Init();
            m_bodyManager.OnBodyPartHit += OnBodyHit;
        }
        public virtual void ReturnToPool()
        {
            m_bodyManager.Shutdown();
            Origin?.ReturnToPool(this);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            OnClash(collision);
        }

        public virtual void OnClash(Collision2D collision)
        {
            Interactable incomingInterable = collision.gameObject.GetComponent<Interactable>();
            if (incomingInterable != null)
            {
                ContactPoint2D contact = collision.GetContact(0);
                Vector2 dir = contact.normal.normalized;
                OnKnockback(dir, incomingInterable.CollisionData.Knockback);
            }
        }

        public virtual void OnKnockback(Vector2 source, float knockback)
        {
            Vector2 diff = RigidBody.position - source;
            m_rigidbody.AddForce(knockback * diff.normalized, ForceMode2D.Impulse);
        }

        protected virtual void OnTakeDamage(float damage, int partDamage = 0) 
        {
        }

        protected virtual void OnBodyHit(BodyHitResult hit)
        {
            OnTakeDamage(hit.Damage, hit.PartDamage);
            OnKnockback(hit.KnockbackSource, hit.KnockbackMod);
            if (hit.PartBreak) 
            {
                OnBodyPartBreak(hit.BodyPart);
            }
            if (hit.WeakpointBreak) 
            {
                OnWeakpointBreak(hit.BodyPart);
            }
        }

        protected virtual void OnBodyPartBreak(BodyPart part) 
        {   
        }

        protected virtual void OnWeakpointBreak(BodyPart part)
        {
        
        }

        protected virtual void OnDefeat()
        {
            Despawn();
        }

        protected virtual void UpdatePathfinder()
        {
            Bounds bounds = GetComponent<Collider2D>().bounds;
            AstarPath.active.UpdateGraphs(bounds);
        }

        protected void Despawn() 
        {
            ReturnToPool();
            if (Origin == null)
            {
                Destroy(gameObject);
            }
        }
    }
}
