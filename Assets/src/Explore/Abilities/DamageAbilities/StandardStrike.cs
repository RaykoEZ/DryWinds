using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class StandardStrike : BaseAbility, IActiveAbility
    {
        [SerializeField] DealDamage_EffectResource m_basicAttack = default;
        protected List<IStatusAilment> m_onHit = new List<IStatusAilment> { };
        public int BaseDamage => m_basicAttack.DamageModule.BaseDamage;
        public IReadOnlyList<IStatusAilment> OnHitEffects => m_onHit;
        public override AbilityContent AbilityDetail
        {
            get
            {
                AbilityContent ret = base.AbilityDetail;
                ret.Name = m_resource.Content.Name;
                ret.Description = $"Deal {BaseDamage} damage to target.";
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
