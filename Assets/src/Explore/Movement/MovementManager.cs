using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using System.Collections.Generic;
using TMPro;

namespace Curry.Explore
{
    public delegate void OnAdventureFinish();
    public class MovementManager: SceneInterruptBehaviour 
    {
        [SerializeField] protected Adventurer m_player = default;
        [SerializeField] protected AdventButton m_moveButton = default;
        [SerializeField] protected EncounterManager m_encounter = default;
        [SerializeField] protected TimeManager m_time = default;
        [SerializeField] protected Tilemap m_terrain = default;
        [SerializeField] protected FogOfWar m_fog = default;
        [SerializeField] protected CurryGameEventListener m_onAdventure = default;
        [SerializeField] protected CurryGameEventListener m_onPlayerMoved = default;
        [SerializeField] protected TextMeshProUGUI m_moveCountText = default;
        public event OnActionStart OnStart;
        public event OnAdventureFinish OnFinish;
        bool m_movementInProgress = false;
        public static readonly string[] s_gameplayCollisionFilters = new string[] 
        { "Player", "Enemies", "Obstacles" };
        void Start()
        {
            m_onAdventure?.Init();
            m_onPlayerMoved?.Init();
            m_player.OnMoveFinished += OnPlayerMovementFinish;
            m_player.OnBlocked += OnPlayerBlocked;
            UpdateMoveCountDisplay();
        }
        void OnPlayerBlocked(Vector3 blocked) 
        {
            Vector3Int cell = m_terrain.WorldToCell(blocked);
            Vector3 cellPos = m_terrain.GetCellCenterWorld(cell);
            m_player.transform.position = new Vector3(cellPos.x, cellPos.y, transform.position.z);
        }
        void OnPlayerMovementFinish(ICharacter player) 
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
            bool tileExist = WorldTile.TryGetTile(m_terrain, worldPos, out WorldTile tile);
            // Trigger player to move to selected tile
            Vector3Int cell = m_terrain.WorldToCell(worldPos);
            Vector3 cellCenter = m_terrain.GetCellCenterWorld(cell);
            // Check for visible obstructions and time
            if (tileExist && !IsPathObstructed(cellCenter, m_player.WorldPosition) && m_time.TrySpendTime(tile.Difficulty))
            {
                List<IEnumerator> action = new List<IEnumerator>
                {
                    StartAdventure(cellCenter, tile)
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
                HandleEncounterEvent(player.Character.WorldPosition);
                OnFinish?.Invoke();
            }
        }
        public void UpdateMoveCounter(int change = 1) 
        {
            m_player.UpdateMoveLimit(change);
            UpdateMoveCountDisplay();
        }
        void UpdateMoveCountDisplay() 
        {
            m_moveCountText.text = $" {m_player.CurrentMoveCount} / {m_player.MaxMoveCount}";
        }
        public void EnablePlay()
        {
            m_moveButton.Interactable = m_player.CanMove;
        }
        public void DisablePlay()
        {
            m_moveButton.Interactable = false;
        }
        // Do a collision check for direct path ahead, if there are hidden obstaclesm we allow the move
        public bool IsPathObstructed(Vector3 targetCellCenter, Vector3 origin) 
        {
            // Trigger player to move to selected tile
            Vector3 diff = targetCellCenter - origin;
            var hit = Physics2D.CircleCastAll
                (origin, 
                0.5f, 
                diff.normalized, 
                Vector2.Distance(origin, 
                targetCellCenter), 
                LayerMask.GetMask(s_gameplayCollisionFilters));
            bool allObstaclesAreUnKnown = true;
            Vector3 pos;
            Vector3Int coord;
            foreach (var obstacle in hit)
            {
                // If even one obstacle is visible, we prevent movement
                if (obstacle.transform.TryGetComponent(out IPlayer _) || 
                    obstacle.transform.TryGetComponent(out IStepOnTrigger _))
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
            return hit.Length > 1 && !allObstaclesAreUnKnown;
        }
        IEnumerator StartAdventure(Vector3 targetPos, WorldTile tile)
        {
            StartInterrupt();
            m_movementInProgress = true;          
            m_player.Move(targetPos);
            UpdateMoveCounter(-1);
            yield return new WaitUntil(() => !m_movementInProgress);
            bool trigger = false;
            OnEncounterFinish encounterFinishTrigger = () => trigger = true;
            m_encounter.OnEncounterFinished += encounterFinishTrigger;
            // after movement, trigger any events
            if (HandleEncounterEvent(m_player.WorldPosition))
            {
                yield return new WaitUntil(() => trigger);
                m_encounter.OnEncounterFinished -= encounterFinishTrigger;
            }           
            EndInterrupt();
        }
        // one time events in locations
        bool HandleEncounterEvent(Vector3 worldPosition)
        {
            // If there are special events in this location, trigger them
            return m_encounter.OnEncounter(worldPosition);
        }
    }
}
