using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class DealDamageTo : PropertyAttribute, ICharacterEffectModule, ITargetEffectModule
    {
        [SerializeField] int m_damage = default;
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            target.TakeHit(m_damage);
        }

        public void ApplyEffect(ICharacter target)
        {
            target.TakeHit(m_damage);
        }
    }
}
