using UnityEngine;
using Curry.Game;
using System;
using System.Collections.Generic;
using Curry.Util;

namespace Curry.Explore
{
    [Serializable]
    public class Reinforcement : PropertyAttribute, ISummonModule
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
    [Serializable]
    public class SpawnInRange : Reinforcement 
    {
        [SerializeField] int m_numToSpawn = default;
        [SerializeField] RangeMap m_spawnRange = default;
        public int NumToSpawn { get { return m_numToSpawn; } set { m_numToSpawn = value; } }
        // Uses targetpos as origin, selects a random position in range pattern to spawn an instance
        public override void ApplyEffect(Vector3 targetWorldPos, PoolableBehaviour spawnRef, Action<PoolableBehaviour> onInstance = null)
        {
            List<Vector3> positions = m_spawnRange.ApplyRangeOffsets(targetWorldPos);
            List<Vector3> spawnPos = GameUtil.SampleFromList(positions, NumToSpawn);
            foreach(Vector3 pos in spawnPos) 
            {
                base.ApplyEffect(pos, spawnRef, onInstance);
            }
        }
    }
}
