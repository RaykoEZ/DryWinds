using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Curry.Game;
using UnityEngine.Tilemaps;
using Curry.UI;
using Curry.Util;
using static UnityEngine.EventSystems.EventTrigger;

namespace Curry.Explore
{
    [Serializable]
    public struct TacticalSpawnProperties 
    {
        [SerializeField] public LayerMask DoNotSpawnOn;
        [SerializeField] public Tilemap SpawnMap;
        [SerializeField] public CurryGameEventListener OnSpawn;
        [SerializeField] public BaseBehaviourInstanceManager InstanceManager;
    }
    public delegate void OnEnemyActionFinish();
    public delegate void OnEnemyActionStart(List<IEnumerator> actions);
    // Monitors all spawned enemies, triggers enemy action phases
    public class EnemyManager : SceneInterruptBehaviour
    {
        #region Enemy Comparer class
        // Comparer<TacticalEnemy> for priority sorting 
        protected class EnemyPriorityComparer : IComparer<IEnemy>
        {
            public int Compare(IEnemy x, IEnemy y)
            {
                if (x.Id == y.Id)
                {
                    return 0;
                }
                if (x == null)
                {
                    if (y == null)
                    {
                        // If x is null and y is null, they're
                        // equal.
                        return 0;
                    }
                    else
                    {
                        // If x is null and y is not null, y
                        // is greater.
                        return -1;
                    }
                }
                else
                {
                    // If x is not null...
                    //
                    if (y == null)
                    // ...and y is null, x is greater.
                    {
                        return 1;
                    }
                    else
                    {
                        // ...and y is not null, return countdown difference
                        return x.Speed - y.Speed;
                    }
                }
            }
        }
        #endregion

        #region Serialize Fields & Members
        [SerializeField] MovementManager m_movement = default;
        [SerializeField] TacticalSpawnProperties m_spawnProperties = default;
        [SerializeField] FogOfWar m_fog = default;
        [SerializeField] RangePreviewHandler m_dangerZoneDisplay = default;
        List<IEnemy> m_activeEnemies = new List<IEnemy>();
        HashSet<IEnemy> m_toRemove = new HashSet<IEnemy>();
        HashSet<IEnemy> m_toAdd = new HashSet<IEnemy>();
        // Comparer for enemy priority
        EnemyPriorityComparer m_priorityComparer = new EnemyPriorityComparer();
        public event OnEnemyActionFinish OnActionFinish;
        public event OnEnemyActionStart OnActionBegin;
        public bool AreEnemiesActive => m_activeEnemies.Count > 0;
        #endregion
        #region Class Body
        void Awake()
        {
            m_spawnProperties.OnSpawn?.Init();
        }
        void Start()
        {
            // Add all intially spawned enemies into the manager
            foreach(Transform t in m_spawnProperties.InstanceManager.PoolDefaultParent) 
            { 
                if (t.TryGetComponent(out IEnemy enemy) && t.TryGetComponent(out PoolableBehaviour behaviour)) 
                {
                    InitInstance(behaviour, enemy.WorldPosition);
                }
            }    
        }
        #region Spawning
        // When a spawner requests an enemy spawn
        public void SpawnEnemy(EventInfo info)
        {
            if (info is SpawnInfo spawn)
            {
                SpawnEnemy_Internal(spawn.Behaviour, spawn.SpawnWorldPosition,spawn.OnInstantiate, spawn.Parent);
            }
        }
        protected void SpawnEnemy_Internal(PoolableBehaviour behaviour, Vector3 position, Action<PoolableBehaviour> setup = null, Transform parent = null)
        {
            Vector3Int coord = m_spawnProperties.SpawnMap.WorldToCell(position);
            Vector3 cellCenter = m_spawnProperties.SpawnMap.GetCellCenterWorld(coord);
            if (!(behaviour is IEnemy) || !m_spawnProperties.SpawnMap.HasTile(coord))
            {
                return;
            }
            // Look for available pooled instances
            PoolableBehaviour newBehaviour = m_spawnProperties.InstanceManager.
                GetInstanceFromAsset(behaviour.gameObject, parent);
            // setup new spawn instance
            setup?.Invoke(newBehaviour);
            InitInstance(newBehaviour, cellCenter);
        }
        void InitInstance(PoolableBehaviour newBehaviour, Vector3 cellCenterWorld) 
        {
            // setup new spawn instance
            newBehaviour.gameObject.transform.position = cellCenterWorld;
            newBehaviour.TryGetComponent(out IEnemy spawn);
            if (spawn is IMovableEnemy movable) 
            {
                movable.OnMove += OnEnemyMovement;
                movable.OnBlocked += OnMovementBlocked;
            }
            spawn.OnDefeat += OnEnemyRemove;
            spawn.OnReveal += OnEnemyReveal;
            spawn.OnHide += OnEnemyHide;
            m_toAdd.Add(spawn);
        }
        void OnEnemyRemove(ICharacter remove)
        {
            if(remove is IMovableEnemy movable) 
            {
                movable.OnMove -= OnEnemyMovement;
                movable.OnBlocked -= OnMovementBlocked;
            }
            remove.OnDefeat -= OnEnemyRemove;
            remove.OnReveal -= OnEnemyReveal;
            remove.OnHide -= OnEnemyHide;
            m_toRemove.Add(remove as IEnemy);
            remove.Despawn();
        }
        #endregion
        #region IEnemy event handlers
        void OnEnemyMovement(IEnemy move, Vector3 destination, Action<Vector3> call) 
        { 
            if(!m_movement.IsPathObstructed(destination, move.WorldPosition)) 
            {
                call?.Invoke(destination);
            }          
        }
        void OnEnemyReveal(ICharacter reveal)
        {
            m_fog.SetFogOfWar(reveal.WorldPosition, clearFog: true);
        }
        void OnEnemyHide(ICharacter hide)
        {
            m_fog.SetFogOfWar(hide.WorldPosition, clearFog: false);
        }

        void OnMovementBlocked(Vector3 pos) 
        {
            m_fog.SetFogOfWar(pos);
        }
        #endregion
        // Whenever player spends time, update all enemies with countdowns 
        // returns: whether there are Responses from active enemies
        // out: enemy responses
        public bool OnEnemyInterrupt(ActionCost resourceSpent, out List<IEnumerator> resp)
        {
            // Update all enemy countdowns here and get all responses
            resp = HandleAction(resourceSpent, reaction: true);
            return resp.Count > 0;
        }
        // Notifies call stack for enemy action calls
        public bool OnEnemyAction() 
        {
            List<IEnumerator> actions = HandleAction(new ActionCost { }, reaction: false);
            bool hasActions = actions.Count > 0;
            if (hasActions) 
            {
                OnActionBegin?.Invoke(actions);
            }
            return actions.Count > 0;
        }

        // add and remove scheduled updates to the enemy list
        void UpdateActivity() 
        {
            foreach (IEnemy e in m_toAdd)
            {
                m_activeEnemies.Add(e);
            }
            foreach (IEnemy e in m_toRemove) 
            {
                m_activeEnemies.Remove(e);
            }
            m_toAdd.Clear();
            m_toRemove.Clear();
        }

        // Call all active enemies to respond to player action
        List<IEnumerator> HandleAction(ActionCost dt, bool reaction) 
        {
            // make sure list is up to date before and after
            UpdateActivity();
            List<IEnumerator> calls = new List<IEnumerator>();
            // sort execution order by ascending countdown value
            m_activeEnemies.Sort(m_priorityComparer);
            foreach (IEnemy enemy in m_activeEnemies)
            {
                // returns true if countdown reached, add to execution list
                if (enemy.OnAction(dt, reaction, out IEnumerator chosenAction) && chosenAction != null)
                {
                    calls.Add(chosenAction);
                }
                StartCoroutine(DisplayDangerZone(enemy.WorldPosition, enemy.IntendingAbility));
            }      
            if (calls.Count > 0) 
            {
                calls.Add(FinishActionPhase());
            }
            UpdateActivity();
            return calls;
        }
        List<string> m_dangerZoneDisplayId = new List<string>();
        IEnumerator DisplayDangerZone(Vector3 origin, AbilityContent ability) 
        {
            if (ability != null && ability != AbilityContent.None) 
            {
                m_dangerZoneDisplayId.Add(m_dangerZoneDisplay?.BeginDisplay(origin, ability));
            }
            else 
            {

            }
            yield return new WaitForEndOfFrame();
        }
        IEnumerator FinishActionPhase() 
        {
            OnActionFinish?.Invoke();
            yield return new WaitForEndOfFrame();
        }
        #endregion
    }
}