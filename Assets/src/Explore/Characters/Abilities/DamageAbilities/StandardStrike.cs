using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class StandardStrike : BaseAbility, IAbility, IDamageAbility
    {
        [SerializeField] DealDamageTo m_basicAttack = default;
        public int Damage => m_basicAttack.BaseDamage + m_basicAttack.AddDamage;
        public override RangeMap Range => RangeMap.Adjacent;
        public override AbilityContent GetContent()
        {
            AbilityContent ret = base.GetContent();
            ret.Name = "Standard Strike";
            ret.Description = $"Deal {Damage} (+{m_basicAttack.AddDamage}) damage to target.";
            return ret;
        }

        public void Activate(ICharacter target) 
        {
            m_basicAttack?.ApplyEffect(target);
        }

        public void AddDamage(int val)
        {
            throw new NotImplementedException();
        }
    }
}
