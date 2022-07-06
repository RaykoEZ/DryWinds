using UnityEngine;
using UnityEngine.Tilemaps;
namespace Curry.Explore
{
    public class AdventCard : MonoBehaviour 
    {
        public int Id = 0;
        public string Name = default;
        public string Description = default;
        public virtual void Activate() 
        { 
        
        }
    }


    public class World : MonoBehaviour 
    {
        [SerializeField] Tilemap m_tileMap = default;

        public T GetTile<T>(Vector2 worldPos) where T : Tile 
        {
            Vector3Int p = m_tileMap.WorldToCell(worldPos);
            return m_tileMap.GetTile<T>(p);
        }
    }

    public class WorldTile : Tile 
    {
        [SerializeField] public int DeckId = default;

        
    }
}
