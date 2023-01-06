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
        [SerializeField] LayerMask m_doNotSpawnOn = default;
        [SerializeField] Tilemap m_spawnMap = default;
        [SerializeField] CurryGameEventTrigger m_spawnTrigger = default;
        public void Spawn(Vector3Int spawnCoord, PoolableBehaviour spawnRef, Transform parent = null)
        {
            if (
                spawnRef == null || 
                !m_spawnMap.HasTile(spawnCoord)) 
            {
                return;
            }
            Vector3 coordWorldPos = m_spawnMap.GetCellCenterWorld(spawnCoord);
            coordWorldPos.z = -1f;
            Vector3 origin = spawnCoord - new Vector3(0f, 0f, 10f);
            bool hit = Physics.Raycast(origin, Vector3.forward, 10f, m_doNotSpawnOn);
            if (!hit)
            {
                EnemySpawnInfo info = new EnemySpawnInfo(coordWorldPos, spawnRef, parent);
                m_spawnTrigger?.TriggerEvent(info);
            }
        }
    }
}