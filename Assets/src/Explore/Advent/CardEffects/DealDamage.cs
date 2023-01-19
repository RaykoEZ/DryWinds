using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class DealDamage : IEffectModule
    {
        [SerializeField] int m_damage = default;
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            target.TakeHit(m_damage);
        }
    }
}
