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
            WorldTile tile, out IReadOnlyDictionary<int, AdventCard> result) 
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
            IReadOnlyDictionary<int, AdventCard> deck;
            WorldTile tile = GetTile<WorldTile>(worldPosition);
            // check for any existing deck
            bool deckExist = TryGetAdventInCollection(tile, out deck) &&
                deck?.Count > 0;
            if (deckExist)
            {
                List<int> cardIds = new List<int>(deck.Keys);
                // Draw advent card from fetched deck
                List<AdventCard> draw = DrawFromDeck(cardIds, tile.ActivityLevel);
                OnDraw?.Invoke(draw);
            }
            else
            {
                Debug.Log("Nothing to see here");
            }
        }

        // For drawing/spawning card object instances with provided card ID
        List<AdventCard> DrawFromDeck(List<int> cardIds, int numToDraw)
        {
            // Draw random cards from a list of cards
            List<int> cardIdsToDraw = new List<int>();
            int rand;
            for (int i = 0; i < numToDraw; ++i)
            {
                rand = UnityEngine.Random.Range(0, cardIds.Count - 1);
                cardIdsToDraw.Add(cardIds[rand]);
            }
            // Get the instance for each of the cards drawn
            List<AdventCard> ret = new List<AdventCard>();
            foreach (int id in cardIdsToDraw)
            {
                // Instantiating cards to be drawn
                AdventCard cardInstance = GetAdventInstance(id);
                ret.Add(cardInstance);
            }
            return ret;
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
