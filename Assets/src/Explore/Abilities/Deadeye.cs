using Curry.Game;
using Curry.Vfx;
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
        Enhancement m_enhanceInstance;
        VfxManager.VfxHandle m_debuffVfx;
        public override AbilityContent AbilityDetail
        {
            get
            {
                var ret = base.AbilityDetail;
                ret.Name = "Deadeye";
                ret.Description = $"When target is in specific positions, increase attack damage by {m_enhancedBuff.DamageUp} and apply [{m_impediment.Content.Name}] on projectile hit.";
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
                HasValidTargets(user.WorldPosition, 
                m_resource.Content.TargetingRange, out validTargets);

            IModifiable modify = user as IModifiable;
            if (rangeCheck) 
            {
                // apply impede on projectile impact
                ranged.RangedAttack?.AddOnHitEffect(new Impediment(m_impediment));
                // Only apply enhanced when user isn't a;ready enhanced
                if( m_enhanceInstance == null || !modify.ContainsModifier(m_enhanceInstance)) 
                {
                    m_enhanceInstance = new Enhancement(m_enhancedBuff);
                    m_enhancedBuff.OnExpire += OnDebuffExpire;
                    // apply damage up
                    modify.ApplyModifier(m_enhanceInstance, m_enhanceInstance.Vfx, m_enhanceInstance.VfxTimeline, out m_debuffVfx);
                    m_debuffVfx?.PlayVfx();
                }
            }
            return rangeCheck;
        }
        void OnDebuffExpire(IStatModifier<TacticalStats> _)
        {
            m_enhancedBuff.OnExpire -= OnDebuffExpire;
            m_debuffVfx?.StopVfx();
            m_enhanceInstance = null;
        }
    }
}
