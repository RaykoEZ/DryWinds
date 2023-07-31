using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;

namespace Curry.Explore
{
    [Serializable]
    public class SeedOfPlenty : CardResource, IHandEffect, IEndOfTurnEffect 
    {
        [SerializeField] Heal_EffectResource m_heal = default;
        [SerializeField] GainStat_EffectResource m_gainStats = default;
        StatUp m_currentModifier;
        public override bool IsActivatable(GameStateContext c)
        { return false; }
        public SeedOfPlenty(SeedOfPlenty effect) : base(effect)
        {
            m_heal = effect.m_heal;
            m_gainStats = effect.m_gainStats;
        }
        public IEnumerator OnEndOfTurn(GameStateContext c)
        {
            yield return EndOfTurn_Internal(c.Player);
        }
        IEnumerator EndOfTurn_Internal(IPlayer player)
        {
            yield return player.TriggerVfx(m_vfx, m_vfxTimeLine);
            m_heal.Healing.ApplyEffect(player);
        }
        IEnumerator HandEffect_Internal(IPlayer player) 
        {
            if (player is IModifiable mod)
            {
                m_currentModifier = new StatUp(m_gainStats.Effect);
                mod.ApplyModifier(m_currentModifier, 
                    m_currentModifier.Vfx, m_currentModifier.VfxTimeline);
                yield return null;
            }
        }
        public IEnumerator HandEffect(GameStateContext c)
        {
            yield return HandEffect_Internal(c.Player);
        }
        public IEnumerator OnLeaveHand(GameStateContext c)
        {
            if (c.Player is IModifiable mod)
            {
                mod.CurrentStats.RemoveModifier(m_currentModifier);
            }
            yield return null;
        }
    }
}