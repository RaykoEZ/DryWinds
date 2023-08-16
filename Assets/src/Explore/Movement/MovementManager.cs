using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using System.Collections.Generic;
using TMPro;
using Curry.Game;
using Curry.UI;

namespace Curry.Explore
{
    public delegate void OnAdventureFinish();
    public class MovementManager: SceneInterruptBehaviour 
    {
        [SerializeField] protected Adventurer m_player = default;
        [SerializeField] protected TextMessageDisplay m_message = default;
        [SerializeField] protected ObstacleAlertHandler m_alert = default;
        [SerializeField] protected EncounterManager m_encounter = default;
        [SerializeField] protected Tilemap m_terrain = default;
        [SerializeField] protected Tilemap m_wall = default;
        [SerializeField] protected FogOfWar m_fog = default;
        [SerializeField] protected ActionCostHandler m_actionCost = default;
        [SerializeField] protected CurryGameEventListener m_onAdventure = default;
        public event OnActionStart OnStart;
        bool m_movementInProgress = false;
        public static readonly string[] s_gameplayCollisionFilters = new string[] 
        { "Player", "Enemies", "Obstacles" };
        void Start()
        {
            m_onAdventure?.Init();
        }
        // Used for event listener handle when player chooses a position to move into
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
                worldPos = select.ClickWorldPos;
            }
            else
            {
                return;
            }
            // look for move target, player moves by default
            object characterObj = info.Payload?["toMove"];
            ICharacter toMove = characterObj != null && characterObj is ICharacter character ?
                character : m_player;
            IEnumerator onMoveFinish = info.Payload?["onMoveFinish"] as IEnumerator;
            MoveCharacter(toMove, worldPos, ActionCostHandler.BaseMovementCost, onMoveFinish);
        }
        // Do a collision check for direct path ahead, if there are hidden obstaclesm we allow the move
        public bool IsPathObstructed(Vector3 targetCellCenterWorld, Vector3 origin, bool alertPlayer = false) 
        {
            // find any obstacles between player and destination
            Vector3 diff = targetCellCenterWorld - origin;
            Vector3 diffnorm = diff.normalized;
            var hit = Physics2D.CircleCastAll
                (origin, 
                0.5f,
                diffnorm, 
                Vector2.Distance(origin, 
                targetCellCenterWorld), 
                LayerMask.GetMask(s_gameplayCollisionFilters));
            bool allObstaclesAreUnKnown = true;
            Vector3 pos;
            Vector3Int coord;
            // used to access the obstructing grid when collision hits an edge on a tile map
            Vector2 gridPosShift = new Vector2(0.1f * diffnorm.x, 0.1f * diffnorm.y);
            foreach (var obstacle in hit)
            {
                // If even one obstacle is visible, we prevent movement
                // ignore player or traps/step event triggers
                if (obstacle.transform.TryGetComponent(out IPlayer _) || 
                    obstacle.transform.TryGetComponent(out IStepOnTrigger _))
                {
                    continue;
                }
                else
                {

                    //check if fog of war is active on this obstacle position
                    pos = obstacle.point + gridPosShift;
                    coord = m_terrain.WorldToCell(pos);
                    bool wallCheck = m_wall.HasTile(coord);
                    bool isClear = m_fog.IsCellClear(coord);
                    if (wallCheck || isClear)
                    {
                        allObstaclesAreUnKnown = false;
                        m_alert.AlertObstacle(m_terrain.GetCellCenterWorld(coord), alertPlayer);
                        m_fog.SetFogOfWar(pos);
                        break;
                    }
                }
            }
            // if there is one or more visible obstacle(s) in the path, we return true for path obstructed
            return hit.Length > 1 && !allObstaclesAreUnKnown;
        }
        public void MoveCharacter(
            ICharacter toMove, 
            Vector3 destination, 
            ActionCost cost,
            IEnumerator onMoveFinish = null)
        {
            StartInterrupt();
            bool tileExist = WorldTile.TryGetTile(m_terrain, destination, out WorldTile tile);
            // Trigger player to move to selected tile
            Vector3Int cell = m_terrain.WorldToCell(destination);
            Vector3 cellCenter = m_terrain.GetCellCenterWorld(cell);
            bool isBlocked = IsPathObstructed(cellCenter, m_player.WorldPosition, true);
            bool eneoughResource = m_actionCost.HasEnoughResource(cost, true);
            // Check for visible obstructions and time
            if (tileExist && !isBlocked && eneoughResource)
            {
                List<IEnumerator> action = new List<IEnumerator>
                {
                    StartAdventure(toMove, cellCenter, cost, onMoveFinish),
                };
                // Trigger player to move to selected tile
                OnStart?.Invoke(
                    new ActionCost { ActionPoint = 1, Time = 1 },
                    action);
            }
            else
            {
                m_message?.OnGameMessage(new GameMessageInfo("Cannot move there"));
                EndInterrupt();
            }
        }
        IEnumerator StartMovement(ICharacter toMove, Vector3 destination, IEnumerator onFinish = null) 
        {
            toMove.OnMoveFinished += OnPlayerMovementFinish;
            toMove.OnBlocked += OnPlayerBlocked;
            m_movementInProgress = true;
            toMove.Move(destination);
            yield return new WaitUntil(() => !m_movementInProgress);
            toMove.OnMoveFinished -= OnPlayerMovementFinish;
            toMove.OnBlocked -= OnPlayerBlocked;
            if(onFinish != null) 
            {
                yield return StartCoroutine(onFinish);
            }
        }
        IEnumerator StartAdventure(ICharacter toMove, Vector3 targetPos, ActionCost payCost, IEnumerator onFinish = null)
        {
            StartInterrupt();
            m_actionCost?.TrySpend(payCost);
            m_encounter.DisableEncounter();
            yield return StartCoroutine(StartMovement(toMove, targetPos, onFinish));
            m_actionCost?.CancelPreview();
            // only player gets to trigger encounters after moving
            if (toMove is Adventurer) 
            {
                yield return new WaitForEndOfFrame();
                // after movement, trigger any events
                m_encounter.CheckForEncounter(toMove.WorldPosition);
            }
            EndInterrupt();
        }
        void OnPlayerBlocked(Vector3 blocked)
        {
            Vector3Int cell = m_terrain.WorldToCell(blocked);
            Vector3 cellPos = m_terrain.GetCellCenterWorld(cell);
            m_player.transform.position = new Vector3(cellPos.x, cellPos.y, transform.position.z);
            m_movementInProgress = false;
        }
        void OnPlayerMovementFinish(ICharacter player)
        {
            m_movementInProgress = false;
        }
    }
}
