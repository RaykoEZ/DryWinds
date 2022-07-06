using UnityEngine;
using UnityEngine.Tilemaps;
namespace Curry.Explore
{
    public class World : MonoBehaviour 
    {
        [SerializeField] protected AdventDatabase m_adventDb = default;
        [SerializeField] protected Tilemap m_tileMap = default;

        void Awake()
        {
            m_adventDb.Init();
        }

        public T GetTile<T>(Vector2 worldPos) where T : WorldTile
        {
            Vector3Int p = m_tileMap.WorldToCell(worldPos);
            return m_tileMap.GetTile<T>(p);
        }

        
    }
}
