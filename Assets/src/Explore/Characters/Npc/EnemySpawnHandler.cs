using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using Curry.Game;

namespace Curry.Explore
{
    public class EnemySpawnInfo : EventInfo
    {
        public Vector3 SpawnWorldPosition { get; protected set; }
        public PoolableBehaviour Behaviour { get; protected set; }
        public Transform Parent { get; protected set; }
        public EnemySpawnInfo(Vector3 pos, PoolableBehaviour behaviour, Transform parent = null)
        {
            SpawnWorldPosition = pos;
            Behaviour = behaviour;
            Parent = parent;
        }
    }
    // Checks if enemy can be spawn on a tile, if it can, trigger event to spawn
    public class EnemySpawnHandler : MonoBehaviour
    {
        [SerializeField] CurryGameEventTrigger m_spawnTrigger = default;
        public virtual void Spawn(Vector3 position, PoolableBehaviour spawnRef, Transform parent = null)
        {
            if (spawnRef == null) 
            {
                return;
            }
            EnemySpawnInfo info = new EnemySpawnInfo(position, spawnRef, parent);
            m_spawnTrigger?.TriggerEvent(info);
        }
    }
}