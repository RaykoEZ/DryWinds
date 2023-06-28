using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Deadeye : BaseAbility
    {
        [SerializeField] protected PositionTargetingModule m_deadEyeCheck = default;
        [SerializeField] protected Impediment m_impediment = default;
        [SerializeField] protected Enhancement m_enhancedBuff = default;
        public override AbilityContent Content
        {
            get
            {
                var ret = base.Content;
                ret.Name = "Deadeye";
                ret.Description = $"When target is in specific positions, increase attack damage and apply [{m_impediment.Content.Name}] on projectile hit.";
                return ret;
            }
        }
        public bool TryActivate<T>(ICharacter user, out List<T> validTargets)
        {
            IRangedUnit ranged = user as IRangedUnit;
            if (ranged == null) 
            {
                validTargets = new List<T>();
                return false;
            }
            bool rangeCheck = 
                m_deadEyeCheck.
                HasValidTarget(user.WorldPosition, 
                m_resource.Content.TargetingRange, out validTargets);

            IModifiable modify = user as IModifiable;
            if (rangeCheck) 
            {
                // apply impede on projectile impact
                ranged.RangedAttack?.AddOnHitEffect(m_impediment);
                // apply damage up
                modify?.CurrentStats.ApplyModifier(m_enhancedBuff);
            }
            return rangeCheck;
        }
    }
}
