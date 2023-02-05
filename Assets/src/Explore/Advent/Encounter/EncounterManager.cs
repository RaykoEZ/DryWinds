using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using System.Collections.Generic;

namespace Curry.Explore
{
    public class EncounterInfo : EventInfo 
    { 
        public int EncounterId { get; protected set; }
        public EncounterInfo(int id) 
        {
            EncounterId = id;
        }
    }
    public delegate void OnAdventureFinish();
    public class EncounterManager: MonoBehaviour 
    {
        [SerializeField] EncounterDataBase m_db = default;
        [SerializeField] EncounterUIHandler m_ui = default;
        [SerializeField] GameStateController m_gameState = default;
        [SerializeField] protected TimeManager m_time = default;
        [SerializeField] protected Tilemap m_terrain = default;
        [SerializeField] protected Tilemap m_locations = default;

        [SerializeField] protected CurryGameEventListener m_onAdventure = default;
        [SerializeField] protected CurryGameEventListener m_onPlayerMoved = default;
        [SerializeField] protected CurryGameEventListener m_onEncounter = default;

        [SerializeField] protected CurryGameEventTrigger m_onAdventureMove = default;
        public event OnActionStart OnStart;
        public event OnAdventureFinish OnFinish;
        void Awake()
        {
            m_onAdventure?.Init();
            m_onPlayerMoved?.Init();
            m_onEncounter?.Init();
        }

        public void OnEncounter(EventInfo info) 
        { 
            if(info is EncounterInfo e && m_db.TryGetDetail(e.EncounterId, out EncounterDetail detail)) 
            {
                m_ui.BeginEncounter(detail, m_gameState.GetCurrent());
            }
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
            WorldTile tile = WorldTile.GetTile<WorldTile>(m_terrain, worldPos);
            m_time.TrySpendTime(tile.Difficulty, out bool enough);
            if (enough)
            {
                List<IEnumerator> action = new List<IEnumerator> { StartAdventure(worldPos) };
                // Trigger player to move to selected tile
                OnStart?.Invoke(tile.Difficulty, action);
            }
            else
            {
                Debug.Log("Not enough time to venture into target location");
            }
        }
        IEnumerator StartAdventure(Vector3 targetPos)
        {
            // Trigger player to move to selected tile
            Vector3Int cell = m_terrain.WorldToCell(targetPos);
            PositionInfo e = new PositionInfo(
                    m_terrain.GetCellCenterWorld(cell));
            m_onAdventureMove?.TriggerEvent(e);
            yield return null;
        }
        // When player reached selected tile, draw cards and trigger events
        public void OnPlayerMoved(EventInfo info)
        {
            if (info == null) return;

            if (info is PlayerInfo player)
            {
                SpecialEvents(player.PlayerStats.WorldPosition);
                OnFinish?.Invoke();
            }
        }

        // one time events in locations
        void SpecialEvents(Vector3 worldPosition)
        {
            // If there are special events in this location, trigger them
            if (WorldTile.TryGetTileComponent(m_locations, worldPosition, out SpecialEventHandler e))
            {
                // Remove events after drawing those special event cards
                e.TriggerEvent();
            }
        }
    }

}
