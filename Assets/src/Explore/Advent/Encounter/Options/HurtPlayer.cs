using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "HurtPlayer_", menuName = "Curry/Encounter/Effects/HurtPlayer", order = 2)]
    public class HurtPlayer : EncounterEffect, IHurtModule
    {
        [SerializeField] DealDamageTo m_hurt = default;

        public string ModuleName => nameof(m_hurt);

        public DealDamageTo DamageProperty => m_hurt;

        public override IEnumerator Activate(GameStateContext context)
        {
            yield return new WaitForEndOfFrame();
            m_hurt.ApplyEffect(context.Player, context.Player);
        }
    }
}