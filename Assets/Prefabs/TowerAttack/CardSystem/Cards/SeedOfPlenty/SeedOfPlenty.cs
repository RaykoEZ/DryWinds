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
        // A vfx and director to insert into a card instance for effect sequence when activating a card
        [SerializeField] VisualEffectAsset m_vfx_Heal = default;
        [SerializeField] TimelineAsset m_vfxTimeLine_Heal = default;
        StatUp m_currentModifier;
        public override bool IsActivatable(GameStateContext c)
        { return false; }
        public SeedOfPlenty(SeedOfPlenty effect) : base(effect)
        {
            m_heal = effect.m_heal;
            m_gainStats = effect.m_gainStats;
            m_vfx_Heal = effect.m_vfx_Heal;
            m_vfxTimeLine_Heal = effect.m_vfxTimeLine_Heal;
        }
        public void OnEndOfTurn(GameStateContext c)
        {
            m_vfxHandler.SetupAsset(m_vfx_Heal, m_vfxTimeLine_Heal);
            m_vfxHandler.StartCoroutine(EndOfTurn_Internal(c.Player));
        }
        IEnumerator EndOfTurn_Internal(IPlayer player)
        {
            yield return m_vfxHandler.StartCoroutine(PlayVfx(player, player.WorldPosition));
            m_heal.Healing.ApplyEffect(player);
        }
        IEnumerator HandEffect_Internal(IPlayer player) 
        {
            yield return m_vfxHandler.StartCoroutine(PlayVfx(player, player.WorldPosition));
            if (player is IModifiable mod)
            {
                m_currentModifier = new StatUp(m_gainStats.Effect);
                mod.CurrentStats.ApplyModifier(m_currentModifier);
            }
        }
        public void HandEffect(GameStateContext c)
        {
            m_vfxHandler.SetupAsset(m_vfx, m_vfxTimeLine);
            m_vfxHandler.StartCoroutine(HandEffect_Internal(c.Player));
        }
        public void OnLeaveHand(GameStateContext c)
        {
            if (c.Player is IModifiable mod)
            {
                mod.CurrentStats.RemoveModifier(m_currentModifier);
            }
        }
    }
}