using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;

namespace Curry.Explore
{
    public class EnemySpawnInfo : EventInfo
    {
        public Vector3 SpawnWorldPosition { get; protected set; }
        public TacticalEnemy SpawnRef { get; protected set; }
        public Transform Parent { get; protected set; }
        public EnemySpawnInfo(Vector3 pos, TacticalEnemy spawn, Transform parent = null)
        {
            SpawnWorldPosition = pos;
            SpawnRef = spawn;
            Parent = parent;
        }
    }
    // Checks if enemy can be spawn on a tile, if it can, trigger event to spawn
    public class EnemySpawnHandler : MonoBehaviour
    {
        [SerializeField] LayerMask m_doNotSpawnOn = default;
        [SerializeField] Tilemap m_spawnMap = default;
        [SerializeField] CurryGameEventTrigger m_spawnTrigger = default;
        public void Spawn(Vector3Int spawnCoord, GameObject spawnRef, Transform parent = null)
        {
            if (
                spawnRef == null || 
                !m_spawnMap.HasTile(spawnCoord) || 
                !spawnRef.TryGetComponent(out TacticalEnemy enemy)) 
            {
                return;
            }
            Vector3 coordWorldPos = m_spawnMap.GetCellCenterWorld(spawnCoord);
            coordWorldPos.z = -2f;
            Vector3 origin = spawnCoord - new Vector3(0f, 0f, 10f);
            bool hit = Physics.Raycast(origin, Vector3.forward, 10f, m_doNotSpawnOn);
            Debug.Log(hit);
            if (!hit)
            {
                EnemySpawnInfo info = new EnemySpawnInfo(coordWorldPos, enemy, parent);
                m_spawnTrigger?.TriggerEvent(info);
            }
        }
    }
}