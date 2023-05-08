using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class DealDamageTo : PropertyAttribute
    {
        [SerializeField] int m_baseDamage = default;
        int m_addDamage = 0;
        public int BaseDamage => m_baseDamage;
        public int AddDamage
        {
            get { return m_addDamage; }
            set { m_addDamage = value; }
        }

        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            int result = m_baseDamage + AddDamage;
            if (user is IModifiable modify) 
            {
                var modifiers = modify.CurrentStats.Modifiers;
                foreach(IAttackModifier mod in modifiers) 
                {
                    result = mod.Apply(result);
                }
            }
            target.TakeHit(result);
        }
        public void ApplyEffect(ICharacter target)
        {
            target.TakeHit(m_baseDamage + AddDamage);
        }
    }
}
