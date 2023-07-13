using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class SeedOfPlenty : AdventCard, IEndOfTurnEffect, IHandEffect
    {
        [SerializeField] Heal_EffectResource m_heal = default;
        [SerializeField] GainStat_EffectResource m_gainStats = default;
        StatUp m_currentModifier;
        public override bool IsActivatable(GameStateContext c)
        { return false; }
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {       
            yield return null;
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