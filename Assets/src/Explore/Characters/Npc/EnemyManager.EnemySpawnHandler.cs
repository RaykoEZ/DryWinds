using System;
using UnityEngine;
using Curry.Game;
namespace Curry.Explore
{
    public partial class EnemyManager
    {
        [Serializable]
        protected class EnemySpawnHandler 
        {
            [SerializeField] TacticalSpawnProperties m_spawnProperties = default;

            public TacticalSpawnProperties SpawnProperties { get => m_spawnProperties; set => m_spawnProperties = value; }

            // Instantiate new enemy from reference object
            public PoolableBehaviour SpawnEnemy_Internal(PoolableBehaviour behaviour, Vector3 position, Action<PoolableBehaviour> setup = null, Transform parent = null)
            {
                Vector3Int coord = SpawnProperties.SpawnMap.WorldToCell(position);
                Vector3 cellCenter = SpawnProperties.SpawnMap.GetCellCenterWorld(coord);
                if (behaviour is not IEnemy || !SpawnProperties.SpawnMap.HasTile(coord))
                {
                    return null;
                }
                // Look for available pooled instances
                PoolableBehaviour newBehaviour = SpawnProperties.InstanceManager.
                    GetInstanceFromAsset(behaviour.gameObject, parent);
                // setup new spawn instance
                setup?.Invoke(newBehaviour);
                return newBehaviour;
            }
        }
    }
}