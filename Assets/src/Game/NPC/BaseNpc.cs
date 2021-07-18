using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class BaseNpc : Interactable
    {
        [SerializeField] protected CollisionStats m_collision = default;
        [SerializeField] protected CharacterStats m_stats = default;
        [SerializeField] protected float m_attackInterval = default;

        protected Transform m_target = default;
        
        public override CollisionStats CollisionStats { get { return m_collision; } }
        public CharacterStats CurrentStats { get { return m_stats; } }
        public Transform Target { get { return m_target; } set { m_target = value; } }

        protected float m_attackTimer = 0f;

        protected virtual void Update() 
        {
            m_attackTimer += Time.deltaTime;
            if(m_attackTimer >= m_attackInterval && m_target != null) 
            {
                m_attackTimer = 0f;
                OnAttack();
            }
        }

        protected virtual void OnAttack() 
        {
            Vector2 dir = m_target.position - transform.position;
            m_rigidbody?.AddForce(dir.normalized * m_stats.Speed, ForceMode2D.Impulse);
        }

        public override void OnTakeDamage(float damage)
        {
            m_stats.Stamina -= damage;

            if (m_stats.Stamina <= 0f)
            {
                OnDefeat();
            }
        }

        protected override void OnTouch(Interactable incomingInteraction) 
        {
            if(incomingInteraction.Relations == ObjectRelations.Player) 
            {
                incomingInteraction.OnTakeDamage(CollisionStats.ContactDamage);
            }
        }

        public override void OnDefeat()
        {
            Debug.Log(" Enemy Defeated");           
        }
    }
}
