using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class DealDamageTo : PropertyAttribute, ICharacterEffectModule, ITargetEffectModule
    {
        [SerializeField] int m_baseDamage = default;
        int m_addDamage = 0;
        public int BaseDamage => m_baseDamage;
        public int AddDamage { 
            get { return m_addDamage; } 
            set { m_addDamage = Mathf.Max(value, 0); } }

        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            target.TakeHit(m_baseDamage + AddDamage);
        }
        public void ApplyEffect(ICharacter target)
        {
            target.TakeHit(m_baseDamage + AddDamage);
        }
    }
}
