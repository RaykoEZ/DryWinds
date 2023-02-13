﻿using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "HurtPlayer_", menuName = "Curry/Encounter/Effects/HurtPlayer", order = 2)]
    public class HurtPlayer : EncounterEffect, IEncounterModule
    {
        [SerializeField] DealDamageTo m_damageToPlayer = default;

        public override string[] SerializePropertyNames => new string[] { nameof(m_damageToPlayer) };

        public DealDamageTo DamageProperty => m_damageToPlayer;

        public override IEnumerator Activate(GameStateContext context)
        {
            yield return new WaitForEndOfFrame();
            m_damageToPlayer.ApplyEffect(context.Player, context.Player);
        }
    }
}