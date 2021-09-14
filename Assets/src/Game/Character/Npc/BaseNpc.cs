using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnDefeat();

    public class BaseNpc : BaseCharacter
    {
        protected CharacterContextFactory m_contextFactory = new CharacterContextFactory();
        public event OnDefeat OnDefeated;
        protected virtual void OnEnable()
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
            base.OnDefeat();
        }
    }
}
