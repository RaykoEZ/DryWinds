using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnNpcTakeDamage();
    public delegate void OnDefeat();

    public class BaseNpc : BaseCharacter
    {
        [SerializeField] protected CollisionStats m_collision = default;
        [SerializeField] protected CharacterStats m_stats = default;
        
        protected Transform m_target = default;

        public override CollisionStats CollisionStats { get { return m_collision; } }
        public override CharacterStats CurrentStats { get { return m_stats; } }
        public Transform Target { get { return m_target; } set { m_target = value; } }

        public event OnNpcTakeDamage OnTakingDamage;
        public event OnDefeat OnDefeated;

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
            OnDefeated?.Invoke();
        }
    }
}
