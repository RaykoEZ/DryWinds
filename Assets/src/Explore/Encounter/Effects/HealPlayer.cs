using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "HealPlayer", menuName = "Curry/Encounter/Effects/HealPlayer", order = 3)]
    public class HealPlayer : BaseEncounterEffect, IEncounterModule
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