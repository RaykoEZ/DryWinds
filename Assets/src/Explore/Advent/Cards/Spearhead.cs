using Curry.Util;
using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Spearhead : CardResource, ITargetsPosition, ICooldown
    {
        [SerializeField] AreaAttack_EffectResource m_attack = default;
        [SerializeField] MoveTo_EffectResource m_moveTo = default;
        public Spearhead(Spearhead effect) : base(effect)
        {
            m_attack = effect.m_attack;
            m_moveTo = effect.m_moveTo;
        }

        public override bool IsActivatable(GameStateContext c)
        {
            bool isBlocked = c.Movement.IsPathObstructed(m_targeting.Target, c.Player.WorldPosition);
            return !isBlocked;
        }
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            m_moveTo?.Effect?.ApplyEffect(user, m_targeting.Target, context.Movement, AreaAttack(user));
            yield return null;
        }
        protected IEnumerator AreaAttack(ICharacter user) 
        {
            yield return user.TriggerVfx(m_vfx, m_vfxTimeLine);
            yield return m_attack.Effect.ApplyEffect(user);
        }
    }
}