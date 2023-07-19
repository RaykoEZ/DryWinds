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
            result = CalculateModifiers(result, user);
            DealDamage_Internal(target, result);
        }
        public void ApplyEffect(ICharacter target, ICharacter user, int addDamage)
        {
            int result = m_baseDamage;
            result = CalculateModifiers(result, user);
            result += addDamage;
            DealDamage_Internal(target, result);
        }
        public void ApplyEffect(ICharacter target)
        {
            DealDamage_Internal(target, m_baseDamage);
        }
        // GetResult damamge after attack modifiers
        int CalculateModifiers(int baseDamage, ICharacter user) 
        {
            int ret = baseDamage;
            if (user is IModifiable modify)
            {
                var modifiers = modify.CurrentStats.Modifiers;
                foreach (TacticalModifier mod in modifiers)
                {
                    ret = mod is IAttackModifier attackMod? attackMod.Apply(ret) : ret;
                }
            }
            return ret;
        }
        void DealDamage_Internal(ICharacter target, int damamge) 
        {
            target.TakeHit(damamge);
        }
    }
}
