using System;
using System.Collections;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{
    // Player: does not damage player
    // Enemy: damages player
    [Flags]
    public enum ObjectRelations 
    {
        None = 0,
        Ally = 1,
        Enemy = 1 << 1
    }

    // A basic script for a collidable object 
    public class Interactable : MonoBehaviour, IPoolable, IClashable
    {
        [SerializeField] protected Rigidbody2D m_rigidbody = default;
        [SerializeField] protected ObjectRelations m_relations = default;
        CollisionStats m_defaultCollisionStats = new CollisionStats(0f, 5f);
        public virtual IObjectPool Origin { get; set; }
        public Rigidbody2D RigidBody { get { return m_rigidbody; } }
        public ObjectRelations Relations { get { return m_relations; } }
        public virtual CollisionStats CollisionData { get { return m_defaultCollisionStats; } }

        public virtual void Prepare() 
        { }
        public virtual void ReturnToPool()
        {
            Origin.ReturnToPool(this);
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

        public virtual void OnKnockback(Vector2 direction, float knockback)
        {
            m_rigidbody.AddForce(knockback * direction, ForceMode2D.Impulse);
        }

        public virtual void OnTakeDamage(float damage) 
        {
        }

        public virtual void OnDefeat()
        {
            Despawn();
        }

        protected virtual void UpdatePathfinder()
        {
            Bounds bounds = GetComponent<Collider2D>().bounds;
            AstarPath.active.UpdateGraphs(bounds);
        }

        public void Despawn() 
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
