using Curry.Util;
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Deadeye : BaseAbility, IAbility
    {
        [SerializeField] protected PositionTargetingModule m_deadEyeCheck = default;
        [SerializeField] protected int m_damageUp = default;
        [SerializeField] protected Impediment m_impediment = default;
        public int DamageUp => m_damageUp;
        public TacticalModifier AppyOnHit => m_impediment;

        public override RangeMap Range => m_deadEyeCheck.Range;

        public override AbilityContent GetContent()
        {
            var ret = base.GetContent();
            ret.Name = "Deadeye";
            ret.Description = $"When target is in specific positions, increase attack damage and apply [{m_impediment.Content.Name}] on projectile hit.";
            return ret;
        }
        public bool CheckConditions<T>(ICharacter user, out List<T> validTargets)
        {
             return m_deadEyeCheck.HasValidTarget(user.WorldPosition, out validTargets);
        }

        public void Activate(IProjectile applyTo)
        {
            if (applyTo is IDamageAbility damage) 
            {
                damage.AddDamage(m_damageUp);
            }
            applyTo.AddOnHitEffect(m_impediment);
        }
    }
}
