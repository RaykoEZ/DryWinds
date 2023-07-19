using System;
using UnityEngine;

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
        public void OnEndOfTurn(GameStateContext c)
        {
            m_heal.Activate(c);
        }
        public void HandEffect(GameStateContext c)
        {
            if (c.Player is IModifiable mod)
            {
                m_currentModifier = new StatUp(m_gainStats.Effect);
                mod.CurrentStats.ApplyModifier(m_currentModifier);
            }
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