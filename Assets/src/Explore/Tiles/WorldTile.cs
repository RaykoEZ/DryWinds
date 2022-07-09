using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Curry.Explore
{
    public abstract class WorldTile : Tile
    {
        [SerializeField] protected int m_collectionId = default;
        public int CollectionId { get { return m_collectionId; } }
    }
}