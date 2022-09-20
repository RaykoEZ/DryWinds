using System;
using System.Collections;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{
    // A basic script for a collidable object 
    public abstract class Interactable : MonoBehaviour, IPoolable
    {
        public virtual IObjectPool Origin { get; set; }

        protected virtual void Awake() 
        { 
            // If this object isn't in a pool, init here
            if(Origin == null) 
            {
                Prepare();
            }
        }

        public virtual void Prepare() { }
        public virtual void ReturnToPool()
        {
            Origin?.Reclaim((object)this);
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
                OnKnockback(dir);
            }
        }

        public virtual void OnKnockback(Vector2 source, float knockback = 1f)
        {
        }

        protected virtual void OnTakeDamage(float damage, int partDamage = 0) 
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
            ObjectPool<Interactable>.ReturnToPool(Origin, this);  
        }
    }
}
