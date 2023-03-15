﻿using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using System.Collections.Generic;
using Curry.Game;
using System;
using System.Runtime.InteropServices.ComTypes;

namespace Curry.Explore
{
    public delegate void OnAdventureFinish();
    public class MovementManager: SceneInterruptBehaviour 
    {
        [SerializeField] protected Adventurer m_player = default;
        [SerializeField] protected EncounterManager m_encounter = default;
        [SerializeField] protected TimeManager m_time = default;
        [SerializeField] protected Tilemap m_terrain = default;
        [SerializeField] protected Tilemap m_locations = default;
        [SerializeField] protected FogOfWar m_fog = default;
        [SerializeField] protected CurryGameEventListener m_onAdventure = default;
        [SerializeField] protected CurryGameEventListener m_onPlayerMoved = default;
        public event OnActionStart OnStart;
        public event OnAdventureFinish OnFinish;
        bool m_movementInProgress = false;
        protected static readonly string[] s_collisionFilters = new string[] { "Player", "Enemies", "Obstacles" };
        void Start()
        {
            m_onAdventure?.Init();
            m_onPlayerMoved?.Init();
            m_player.OnMoveFinished += OnPlayerMovementFinish;
            m_player.OnBlocked += OnPlayerBlocked;
        }
        void OnPlayerBlocked(Vector3 blocked) 
        {
            Vector3Int cell = m_terrain.WorldToCell(blocked);
            Vector3 cellPos = m_terrain.GetCellCenterWorld(cell);
            m_player.transform.position = new Vector3(cellPos.x, cellPos.y, transform.position.z);
        }
        void OnPlayerMovementFinish(IPlayer player) 
        {
            m_movementInProgress = false;
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
            StartInterrupt();
            WorldTile tile = WorldTile.GetTile<WorldTile>(m_terrain, worldPos);
            if (tile != null && m_time.TrySpendTime(tile.Difficulty))
            {
                List<IEnumerator> action = new List<IEnumerator>
                {
                    StartAdventure(worldPos)
                };
                // Trigger player to move to selected tile
                OnStart?.Invoke(tile.Difficulty, action);
            }
            else
            {
                EndInterrupt();
                Debug.Log("Unable to move there.");
            }
        }
        // When player reached selected tile, draw cards and trigger events
        public void OnPlayerMoved(EventInfo info)
        {
            if (info == null) return;

            if (info is CharacterInfo player)
            {
                SpecialEvents(player.Character.WorldPosition);
                OnFinish?.Invoke();
            }
        }
        IEnumerator StartAdventure(Vector3 targetPos)
        {
            StartInterrupt();
            m_movementInProgress = true;
            // Trigger player to move to selected tile
            Vector3Int cell = m_terrain.WorldToCell(targetPos);
            Vector3 target = m_terrain.GetCellCenterWorld(cell);
            var hit = Physics2D.LinecastAll(m_player.WorldPosition, target, LayerMask.GetMask(s_collisionFilters));
            bool allObstaclesAreUnKnown = true;
            Vector3 pos;
            Vector3Int coord;
            foreach(var obstacle in hit) 
            {
                // If one obstacle is visible, we prevent collision
                if (obstacle.transform.TryGetComponent( out IPlayer _)) 
                {
                    continue;
                }
                else
                {
                    pos = obstacle.point;
                    coord = m_terrain.WorldToCell(pos);
                    bool isClear = m_fog.IsCellClear(coord);
                    if (isClear) 
                    {
                        allObstaclesAreUnKnown = false;
                        break;
                    }
                }

            }

            if(hit.Length > 1 && !allObstaclesAreUnKnown) 
            {
                Debug.Log("Blocked by obstacles");
            }
            else 
            {
                m_player.Move(target);
                yield return new WaitUntil(() => !m_movementInProgress);
                bool trigger = false;
                OnEncounterFinish encounterFinishTrigger = () => trigger = true;
                m_encounter.OnEncounterFinished += encounterFinishTrigger;
                if (SpecialEvents(m_player.WorldPosition))
                {
                    yield return new WaitUntil(() => trigger);
                    m_encounter.OnEncounterFinished -= encounterFinishTrigger;
                }
            }
            EndInterrupt();
        }


        // one time events in locations
        bool SpecialEvents(Vector3 worldPosition)
        {
            // If there are special events in this location, trigger them
            if (WorldTile.TryGetTileComponent(m_locations, worldPosition, out SpecialEventHandler e))
            {
                // Remove events after drawing those special event cards
                int encounterId = e.EncounterId;
                m_encounter.OnEncounter(encounterId);
                return true;
            }
            else 
            {
                return false;
            }
        }
    }

}
