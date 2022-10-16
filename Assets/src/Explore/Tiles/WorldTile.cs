using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "BaseTerrain", menuName = "Curry/Tiles/Basic", order = 1)]
    public class WorldTile : Tile
    {
        [SerializeField] protected string m_collectionId = default;
        [Range(0, 10)]
        [SerializeField] protected int m_diffculty = default;
        [Range(1, 5)]
        [SerializeField] protected int m_activityLevel = default;
        public string CollectionId { get { return m_collectionId; } }
        public int Traversable { get { return m_diffculty; } }
        public int ActivityLevel { get { return m_activityLevel; } }

    }
}