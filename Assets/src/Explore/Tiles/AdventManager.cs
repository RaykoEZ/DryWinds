using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Game;
namespace Curry.Explore
{
    // Contained database for all available advent(cards and decks)
    // Quiries Tile info for a tile in its tilemap coordinate
    public class AdventManager : MonoBehaviour 
    {
        [SerializeField] protected AdventDatabase m_adventDb = default;
        [SerializeField] protected Tilemap m_tileMap = default;
        [SerializeField] AdventInstanceManager m_instance = default;
        void Awake()
        {
            m_adventDb.Init(OnAdventLoadFinish);
        }

        public T GetTile<T>(Vector2 worldPos) where T : WorldTile
        {
            Vector3Int p = m_tileMap.WorldToCell(worldPos);
            return m_tileMap.GetTile<T>(p);
        }

        public bool TryGetAdventInCollection(
            Vector2 worldPos, out IReadOnlyDictionary<int, AdventCard> result) 
        {
            int retId = GetTile<WorldTile>(worldPos).CollectionId;
            AdventCollection collection;
            bool ret = m_adventDb.AdventCollections.TryGetValue(retId, out collection);
            result = collection?.AdventDictionary;
            return ret;
        }

        public AdventCard GetAdventInstance(int cardId) 
        {
            AdventCard advent = m_adventDb.AdventList[cardId];
            AdventCard ret;
            ret = m_instance.GetInstanceFromAsset(advent.gameObject);
            return ret;
        }

        void OnAdventLoadFinish() 
        {
            Debug.Log("Advent card finished loading, start pooling card assets");
            foreach(KeyValuePair<int, AdventCard> advent in m_adventDb.AdventList) 
            {
                m_instance.PrepareNewInstance(advent.Value.gameObject);                
            }
        }
    }

}
