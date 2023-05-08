using UnityEngine;
using Curry.Game;
using System;

namespace Curry.Explore
{

    [Serializable]
    public class Reinforcement : PropertyAttribute
    {
        [SerializeField] LayerMask m_doNotSpawnOn = default;
        [SerializeField] SpawnHandler m_spawner = default;
        public LayerMask DoNotSpawnOn => m_doNotSpawnOn;
        public virtual void ApplyEffect(Vector3 targetWorldPos, PoolableBehaviour spawnRef, Action<PoolableBehaviour> onInstance = null)
        {
            if (spawnRef == null) return;

            m_spawner.Spawn(targetWorldPos, spawnRef, onInstance);
        }
    }
}
