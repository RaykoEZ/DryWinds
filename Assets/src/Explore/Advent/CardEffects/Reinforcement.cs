using UnityEngine;
using Curry.Game;
using System;

namespace Curry.Explore
{
    [Serializable]
    public class Reinforcement : ITileEffectModule
    {
        [SerializeField] LayerMask m_doNotSpawnOn = default;
        [SerializeField] EnemySpawnHandler m_spawner = default;
        [SerializeField] PoolableBehaviour m_spawnRef = default;
        public LayerMask DoNotSpawnOn => m_doNotSpawnOn;
        public void ApplyEffect(Vector3 targetWorldPos)
        {
            m_spawner.Spawn(targetWorldPos, m_spawnRef);
        }
    }
}
