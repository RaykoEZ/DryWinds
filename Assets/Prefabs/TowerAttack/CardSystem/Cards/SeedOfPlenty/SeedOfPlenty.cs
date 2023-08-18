using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;

namespace Curry.Explore
{
    [Serializable]
    public class SeedOfPlenty : CardResource, IHandEffect, IEffectOverTime 
    {
        [SerializeField] Heal_EffectResource m_heal = default;
        [SerializeField] GainStat_EffectResource m_gainStats = default;
        [SerializeField] int m_baseRegenTimeInterval = default;
        StatUp m_currentModifier;
        int m_timeCounter = 0;
        public int TimeInterval => m_baseRegenTimeInterval;

        public override bool IsActivatable(GameStateContext c)
        { return false; }
        public SeedOfPlenty(SeedOfPlenty effect) : base(effect)
        {
            m_heal = effect.m_heal;
            m_gainStats = effect.m_gainStats;
            m_baseRegenTimeInterval = effect.m_baseRegenTimeInterval;
        }
        public IEnumerator OnTick(GameStateContext c)
        {
            m_timeCounter++;
            if (m_timeCounter >= TimeInterval) 
            {
                m_timeCounter = 0;
                yield return Tick_Internal(c.Player);
            }
        }
        IEnumerator Tick_Internal(IPlayer player)
        {
            if (player.CurrentHp < player.MaxHp) 
            {
                player.TriggerVfx(m_vfx, m_vfxTimeLine);
                m_heal.Healing.ApplyEffect(player);
            }
            yield return null;
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