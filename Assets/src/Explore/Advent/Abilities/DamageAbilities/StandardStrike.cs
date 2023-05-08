using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class StandardStrike : BaseAbility, IActiveAbility
    {
        [SerializeField] DealDamage_EffectResource m_basicAttack = default;
        protected int m_addDamage = 0;
        protected List<IStatusAilment> m_onHit = new List<IStatusAilment> { };
        public int BaseDamage => m_basicAttack.DamageModule.BaseDamage;
        public IReadOnlyList<IStatusAilment> OnHitEffects => m_onHit;
        public override AbilityContent Content
        {
            get
            {
                AbilityContent ret = base.Content;
                ret.Name = "Standard Strike";
                ret.Description = $"Deal {BaseDamage} (+{m_addDamage}) damage to target.";
                return ret;
            }
        }
        public void Activate(ICharacter target) 
        {
            m_basicAttack?.DamageModule?.ApplyEffect(target);
        }
        public void AddOnHitEffect(IStatusAilment mod)
        {
            if (mod == null) return;
            m_onHit.Add(mod);
        }

        public void RemoveOnHitEffect(IStatusAilment mod)
        {
            if (mod == null) return;
            m_onHit.Remove(mod);
        }
    }
}
