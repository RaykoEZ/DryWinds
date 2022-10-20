using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
namespace Curry.Explore
{
    public class CardDrawInfo : EventInfo
    { 
        // All basic cards drawn (excludes all subclass AdventCard instances
        // e.g. Encounters 
        public IReadOnlyList<AdventCard> CardsDrawn { get; protected set; }
        public IReadOnlyList<Encounter> Encounters { get; protected set; }

        public CardDrawInfo SetCardDraw(List<AdventCard> draw) 
        {
            if (draw != null) 
            {
                CardsDrawn = draw;
            }
            return this;
        }

        public CardDrawInfo SetEncounters(List<Encounter> encounters) 
        {
            if (encounters != null) 
            {
                Encounters = encounters;
            }
            return this;
        }
    }

    // Contained database for all available advent(cards and decks)
    // Quiries Tile info for a tile in its tilemap coordinate
    public class AdventManager : MonoBehaviour 
    {
        [SerializeField] protected AdventDatabase m_adventDb = default;
        [SerializeField] protected Tilemap m_terrain = default;
        [SerializeField] protected Tilemap m_locations = default;
        [SerializeField] AdventInstanceManager m_instance = default;
        // When adventure starts and player needs to move to a tile
        [SerializeField] CurryGameEventListener m_onAdventure = default;
        // Trigger this to move player
        [SerializeField] CurryGameEventTrigger m_onAdventureMove = default;
        // When player finishes moving during an adventure
        [SerializeField] CurryGameEventListener m_onPlayerMoved = default;
        // After player moved, we draw card from player position, trigger this
        [SerializeField] CurryGameEventTrigger m_onCardDraw = default;
        [SerializeField] CurryGameEventTrigger m_onDiscardHand = default;

        void Awake()
        {
            m_adventDb.Init(OnAdventLoadFinish);
            m_onAdventure?.Init();
            m_onPlayerMoved?.Init();
        }

        public static T GetTile<T>(Tilemap map, Vector2 worldPos) where T : WorldTile
        {
            Vector3Int p = map.WorldToCell(worldPos);
            return map.GetTile<T>(p);
        }

        public bool TryGetAdventInCollection(
            WorldTile tile, out AdventDeck result) 
        {
            if (tile == null) 
            {
                Debug.LogWarning("Cannot find tile in tilemap");
                result = null;
                return false; 
            }
            string retId = tile.CollectionId;
            AdventDeck collection;
            bool ret = m_adventDb.AdventDecks.TryGetValue(retId, out collection);
            result = collection;
            return ret;
        }



        public void Adventure(EventInfo info)
        {
            if (info == null)
            {
                return;
            }
            // Set destination world position
            Vector3 worldPos;
            if (info is PositionInfo move) 
            {
                worldPos = move.WorldPosition;
            }
            else if (info is TileSelectionInfo select) 
            {
                worldPos = Camera.main.ScreenToWorldPoint(select.ClickScreenPosition);
            }
            else 
            {
                return; 
            }
            // Trigger player to move to selected tile
            Vector3Int cell = m_terrain.WorldToCell(worldPos);
            PositionInfo e = new PositionInfo(
                    m_terrain.GetCellCenterWorld(cell));
            m_onAdventureMove?.TriggerEvent(e);
        }

        // When player reached selected tile, draw cards and trigger events
        public void OnPlayerMoved(EventInfo info) 
        {
            if (info == null) return;

            if (info is PlayerInfo player) 
            {
                m_onDiscardHand?.TriggerEvent(new EventInfo());
                DrawFromMap(m_terrain, player.PlayerStats.WorldPosition);
                DrawFromMap(m_locations, player.PlayerStats.WorldPosition);
                LocationEvents(player.PlayerStats.WorldPosition);
            }
        }

        void LocationEvents(Vector3 worldPosition) 
        {
            LocationTile tile = GetTile<LocationTile>(m_locations, worldPosition);
            if (tile == null)
            {
                return;
            }
            DrawCards(tile.Events);
        }

        void DrawFromMap(Tilemap map, Vector3 worldPosition) 
        {
            AdventDeck deck;
            WorldTile tile = GetTile<WorldTile>(map, worldPosition);
            if (tile == null) 
            { 
                return; 
            }
            // check for any existing deck
            bool deckExist = TryGetAdventInCollection(tile, out deck) &&
                deck?.Cards.Count > 0;

            if (!deckExist)
            {
                Debug.Log("Nothing to see here at: " + map.name);
                return;
            }
            // Get card prefab refs from fetched deck and instantiate
            List<AdventCard> drawRefs = AdventDatabase.DrawCards(deck);
            DrawCards(drawRefs);
        }

        // Instantiate cards and trigger game events OnCardDraw
        void DrawCards(IReadOnlyList<AdventCard> cardsToDraw) 
        {
            List<AdventCard> cardInstances = new List<AdventCard>();
            List<Encounter> encounters = new List<Encounter>();

            foreach (AdventCard cardRef in cardsToDraw)
            {
                // Instantiating cards to be drawn
                AdventCard cardInstance = InstantiateCard(cardRef);
                if (cardInstance is Encounter encounter)
                {
                    encounters.Add(encounter);
                }
                else
                {
                    cardInstances.Add(cardInstance);
                }
            }

            CardDrawInfo info = new CardDrawInfo().
                SetCardDraw(cardInstances).
                SetEncounters(encounters);

            m_onCardDraw?.TriggerEvent(info);
        }

        AdventCard InstantiateCard(AdventCard cardRef)
        {
            AdventCard ret;
            ret = m_instance.GetInstanceFromAsset(cardRef.gameObject);
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
