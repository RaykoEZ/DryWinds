using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnNpcTakeDamage();
    public class BaseNpc : BaseCharacter
    {
        [SerializeField] protected CollisionStats m_collision = default;
        [SerializeField] protected CharacterStats m_stats = default;
        [SerializeField] protected float m_attackInterval = default;
        
        protected float m_attackTimer = 0f;
        protected Transform m_target = default; 
        
        public override CollisionStats CollisionStats { get { return m_collision; } }
        public override CharacterStats CurrentStats { get { return m_stats; } }
        public Transform Target { get { return m_target; } set { m_target = value; } }

        public event OnNpcTakeDamage OnTakingDamage;
        

        protected virtual void Update() 
        {
            m_attackTimer += Time.deltaTime;
            if(m_attackTimer >= m_attackInterval && m_target != null && m_attackInterval > 0) 
            {
                m_attackTimer = 0f;
                OnAttack();
            }
        }

        protected virtual void OnAttack() 
        {
            Vector2 dir = m_target.position - transform.position;
            m_rigidbody?.AddForce(dir.normalized * 10f* m_stats.Speed, ForceMode2D.Impulse);
        }

        protected override void OnClash(Collision2D collision) 
        {
            base.OnClash(collision);
        }

        public override void OnKnockback(Vector2 direction, float knockback) 
        {
            base.OnKnockback(direction, knockback);
        }

        public override void OnTakeDamage(float damage)
        {
            base.OnTakeDamage(damage);
            OnTakingDamage?.Invoke();
        }

        public override void OnDefeat()
        {

        }
    }
}
