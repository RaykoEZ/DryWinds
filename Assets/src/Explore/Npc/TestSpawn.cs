using UnityEngine;

namespace Curry.Explore
{
    public class TestSpawn : MonoBehaviour
    {
        [SerializeField] EnemySpawnHandler m_spawner = default;
        [SerializeField] GameObject m_ref = default;
        private void Start()
        {
            m_spawner.Spawn(new Vector3Int(1, 1, 0), m_ref);
        }
    }
}