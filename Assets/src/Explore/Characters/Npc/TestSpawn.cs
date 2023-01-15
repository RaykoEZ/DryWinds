using UnityEngine;
using Curry.Game;
using System;
using Curry.Events;

namespace Curry.Explore
{
    public class TestSpawn : MonoBehaviour
    {
        [SerializeField] EnemySpawnHandler m_spawner = default;
        [SerializeField] TacticalEnemy m_ref = default;
        private void Start()
        {
            m_spawner.Spawn(transform.position, m_ref);
        }
    }
}