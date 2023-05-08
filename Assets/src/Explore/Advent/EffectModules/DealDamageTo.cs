using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class DealDamageTo : PropertyAttribute
    {
        [SerializeField] int m_baseDamage = default;
        public int BaseDamage => m_baseDamage;
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            int result = m_baseDamage;
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
            target.TakeHit(m_baseDamage);
        }
    }
}
