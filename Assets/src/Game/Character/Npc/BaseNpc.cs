using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnNpcTakeDamage();
    public delegate void OnDefeat();

    public class BaseNpc : BaseCharacter
    {       
        protected Transform m_target = default;
        protected CharacterContextFactory m_contextFactory = new CharacterContextFactory();

        public Transform Target { get { return m_target; } set { m_target = value; } }
        public event OnDefeat OnDefeated;

        protected virtual void Start() 
        {
            m_statsManager.Init(m_contextFactory);
        }

        public override void OnKnockback(Vector2 direction, float knockback) 
        {
            base.OnKnockback(direction, knockback);
        }

        public override void OnDefeat()
        {
            OnDefeated?.Invoke();
        }
    }
}
