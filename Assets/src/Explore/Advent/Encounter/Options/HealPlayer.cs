using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "HealPlayer_", menuName = "Curry/Encounter/Effects/HealPlayer", order = 1)]
    public class HealPlayer : EncounterEffect, IHealModule
    {
        [SerializeField] HealingModule m_heal = default;

        public string ModuleName => nameof(m_heal);

        public HealingModule HealProperty => m_heal;

        public override IEnumerator Activate(GameStateContext context)
        {
            yield return new WaitForEndOfFrame();
            m_heal.ApplyEffect(context.Player, context.Player);
        }
    }
}