using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "BaseTerrain_", menuName = "Curry/Tiles", order = 1)]
    public class WorldTile : Tile
    {
        [SerializeField] protected string m_collectionId = default;
        [Range(0, 10)]
        [SerializeField] protected int m_diffculty = default;
        // Randomly draw from a deck with this ID
        public string CollectionId { get { return m_collectionId; } }
        public int Difficulty { get { return m_diffculty; } }

    }
}