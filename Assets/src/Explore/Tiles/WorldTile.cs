using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "BaseTerrain_", menuName = "Curry/Tiles", order = 1)]
    public class WorldTile : Tile
    {
        [Range(0, 10)]
        [SerializeField] protected int m_diffculty = default;
        // Randomly draw from a deck with this ID
        public int Difficulty { get { return m_diffculty; } }

        public static T GetTile<T>(Tilemap map, Vector2 worldPos) where T : WorldTile
        {
            Vector3Int p = map.WorldToCell(worldPos);
            return map.GetTile<T>(p);
        }
        public static bool TryGetTileComponent<T>(Tilemap map, Vector3 worldPos, out T component) where T : MonoBehaviour
        {
            Vector3Int p = map.WorldToCell(worldPos);
            GameObject obj = map.GetInstantiatedObject(p);
            if (obj == null)
            {
                component = null;
                return false;
            }
            bool ret = obj.TryGetComponent(out T comp);
            component = comp;
            return ret;
        }
    }
}