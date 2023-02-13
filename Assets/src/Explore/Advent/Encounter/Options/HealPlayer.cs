using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "HealPlayer_", menuName = "Curry/Encounter/Effects/HealPlayer", order = 1)]
    public class HealPlayer : EncounterEffect, IEncounterModule
    {
        [SerializeField] HealingModule m_healAmount = default;

        public override string[] SerializePropertyNames => new string[] { nameof(m_healAmount) };

        public HealingModule HealProperty => m_healAmount;

        public override IEnumerator Activate(GameStateContext context)
        {
            yield return new WaitForEndOfFrame();
            m_healAmount.ApplyEffect(context.Player, context.Player);
        }
    }
}