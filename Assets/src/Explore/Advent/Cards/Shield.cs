using Curry.Game;
using Curry.Vfx;
using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Shield : CardResource, ICooldown
    {
        [SerializeField] protected DamageReduction_EffectResource<TemporaryShield> m_damageReduction = default;
        TemporaryShield m_shieldInstance;
        VfxManager.VfxHandle m_buffVfx;
        public Shield(Shield effect) : base(effect)
        {
            m_damageReduction = effect.m_damageReduction;
        }
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            m_shieldInstance = new TemporaryShield(m_damageReduction.Effect);
            m_shieldInstance.OnExpire += OnShieldExpire;
            VfxManager.VfxHandle handle = user.AddVfx(m_vfx, m_vfxTimeLine);
            handle?.PlayVfx();
            if(user is IModifiable mod) 
            {
                mod.ApplyModifier(m_shieldInstance, m_shieldInstance.Vfx, m_shieldInstance.VfxTimeline, out m_buffVfx);
                m_buffVfx?.PlayVfx();
            }
            yield return null;
        }
        void OnShieldExpire(IStatModifier<TacticalStats> _) 
        {
            m_shieldInstance.OnExpire -= OnShieldExpire;
            m_buffVfx?.StopVfx();
        }
    }
}