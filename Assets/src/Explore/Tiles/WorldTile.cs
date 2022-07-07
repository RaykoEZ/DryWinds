using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    public class WorldTile : Tile
    {
        [SerializeField] protected int m_collectionId = default;
        public int CollectionId { get { return m_collectionId; } }
    }
}