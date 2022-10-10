using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
namespace Curry.Explore
{
    

    public delegate void OnCardDraw(List<AdventCard> draw);
    // Contained database for all available advent(cards and decks)
    // Quiries Tile info for a tile in its tilemap coordinate
    public class AdventManager : MonoBehaviour 
    {
        [SerializeField] protected AdventDatabase m_adventDb = default;
        [SerializeField] protected Tilemap m_tileMap = default;
        [SerializeField] AdventInstanceManager m_instance = default;
        [SerializeField] CurryGameEventListener m_onAdventure = default;
        public event OnCardDraw OnDraw;
        void Awake()
        {
            m_adventDb.Init(OnAdventLoadFinish);
            m_onAdventure?.Init();
        }

        public T GetTile<T>(Vector2 worldPos) where T : WorldTile
        {
            Vector3Int p = m_tileMap.WorldToCell(worldPos);
            return m_tileMap.GetTile<T>(p);
        }

        public bool TryGetAdventInCollection(
            WorldTile tile, out AdventCollection result) 
        {
            if (tile == null) 
            {
                Debug.LogWarning("Cannot find tile in tilemap");
                result = null;
                return false; 
            }
            int retId = tile.CollectionId;
            AdventCollection collection;
            bool ret = m_adventDb.AdventCollections.TryGetValue(retId, out collection);
            result = collection;
            return ret;
        }

        public AdventCard InstantiateCard(AdventCard cardRef) 
        {
            AdventCard ret;
            ret = m_instance.GetInstanceFromAsset(cardRef.gameObject);
            return ret;
        }

        public void Adventure(EventInfo info)
        {
            if (info == null)
            {
                return;
            }
            else if (info is TileSelectionInfo select) 
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(select.ClickScreenPosition);
                DrawCardsFrom(worldPos);
            }         
        }

        void DrawCardsFrom(Vector3 worldPosition) 
        {
            AdventCollection deck;
            WorldTile tile = GetTile<WorldTile>(worldPosition);
            // check for any existing deck
            bool deckExist = TryGetAdventInCollection(tile, out deck) &&
                deck?.AdventDictionary.Count > 0;

            if (!deckExist)
            {
                Debug.Log("Nothing to see here");
                return;
            }
            // Draw advent card from fetched deck
            List<AdventCard> drawRefs = AdventDatabase.DrawCards(deck, tile.ActivityLevel);
            List<AdventCard> cardInstances = new List<AdventCard>();
            foreach (AdventCard cardRef in drawRefs)
            {
                // Instantiating cards to be drawn
                AdventCard cardInstance = InstantiateCard(cardRef);
                cardInstances.Add(cardInstance);
            }

            OnDraw?.Invoke(cardInstances);
        }

        void OnAdventLoadFinish() 
        {
            foreach(KeyValuePair<int, AdventCard> advent in m_adventDb.AdventList) 
            {
                m_instance.PrepareNewInstance(advent.Value.gameObject);                
            }
        }
    }

}
