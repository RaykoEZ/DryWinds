using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Curry.Game;
using UnityEngine.Tilemaps;
using Curry.UI;
using Curry.Util;

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
                        return x.CurrentStatus.Speed - y.CurrentStatus.Speed;
                    }
                }
            }
        }
        #endregion

        #region Serialize Fields & Members
        [SerializeField] TacticalSpawnProperties m_spawnProperties = default;
        [SerializeField] CameraManager m_camera = default;
        [SerializeField] FogOfWar m_fog = default;
        List<IEnemy> m_activeEnemies = new List<IEnemy>();
        HashSet<IEnemy> m_toRemove = new HashSet<IEnemy>();
        HashSet<IEnemy> m_toAdd = new HashSet<IEnemy>();
        // Comparer for enemy priority
        EnemyPriorityComparer m_priorityComparer = new EnemyPriorityComparer();
        public event OnEnemyActionFinish OnActionFinish;
        public event OnEnemyActionStart OnActionBegin;
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
            cellCenterWorld.z = -1f;
            newBehaviour.gameObject.transform.position = cellCenterWorld;
            newBehaviour.TryGetComponent(out IEnemy spawn);
            spawn.OnDefeat += OnEnemyRemove;
            spawn.OnReveal += OnEnemyReveal;
            spawn.OnHide += OnEnemyHide;
            spawn.OnBlocked += OnMovementBlocked;
            m_toAdd.Add(spawn);
        }
        #endregion

        #region IEnemy event handlers
        void OnEnemyReveal(IEnemy reveal)
        {
            m_fog.SetFogOfWar(reveal.WorldPosition, clearFog: true);
        }
        void OnEnemyHide(IEnemy hide)
        {
            m_fog.SetFogOfWar(hide.WorldPosition, clearFog: false);
        }
        void OnEnemyRemove(IEnemy remove)
        {
            remove.OnDefeat -= OnEnemyRemove;
            remove.OnReveal -= OnEnemyReveal;
            remove.OnHide -= OnEnemyHide;
            remove.OnBlocked -= OnMovementBlocked;
            m_toRemove.Add(remove);
            remove.OnDefeated();
        }
        void OnMovementBlocked(Vector2 pos) 
        {
            m_fog.SetFogOfWar(pos);
        }
        #endregion
        // Whenever player spends time, update all enemies with countdowns 
        // returns: whether there are Responses from active enemies
        // out: enemy responses
        public bool OnEnemyInterrupt(int timeSpent, out List<IEnumerator> resp)
        {
            // Update all enemy countdowns here and get all responses
            resp = HandleAction(timeSpent, reaction: true);
            return resp.Count > 0;
        }
        // Notifies call stack for enemy action calls
        public bool OnEnemyAction() 
        {
            List<IEnumerator> actions = HandleAction(0, reaction: false);
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
        List<IEnumerator> HandleAction(int dt, bool reaction) 
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
                    calls.Add(PresentActingEnemy(enemy));
                    calls.Add(chosenAction);
                }
            }
            calls.Add(FinishActionPhase());
            UpdateActivity();
            return calls;
        }
        IEnumerator FinishActionPhase() 
        {
            OnActionFinish?.Invoke();
            yield return null;
        }
        IEnumerator PresentActingEnemy(IEnemy e) 
        {
            StartInterrupt();
            m_camera.FocusCamera(e.WorldPosition);
            yield return new WaitForSeconds(m_camera.AnimationTime);
        }
        #endregion
    }
}