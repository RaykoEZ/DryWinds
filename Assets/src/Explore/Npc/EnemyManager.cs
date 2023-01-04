using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    public class EnemyPoolCollection : PoolCollection<PoolableBehaviour>
    {
    }
    [Serializable]
    public class EnemyInstanceManager : InstanceManager<PoolableBehaviour>
    {
        [SerializeField] EnemyPoolCollection m_pool = default;
        protected override PoolCollection<PoolableBehaviour> Pool { get { return m_pool; } }
    }
    // Monitors all spawned enemies, triggers enemy action phases
    public class EnemyManager : MonoBehaviour
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
                        return x.CurrentStatus.AttackCountdown - y.CurrentStatus.AttackCountdown;
                    }
                }
            }
        }
        #endregion
        #region Serialize Fields & Members
        [SerializeField] EnemyInstanceManager m_instance = default;
        [SerializeField] GameClock m_clock = default;
        [SerializeField] FogOfWar m_fog = default;
        [SerializeField] protected CurryGameEventListener m_onSpawn = default;
        List<IEnemy> m_activeEnemies = new List<IEnemy>();
        HashSet<IEnemy> m_toRemove = new HashSet<IEnemy>();
        HashSet<IEnemy> m_toAdd = new HashSet<IEnemy>();
        // Comparer for enemy priority
        EnemyPriorityComparer m_priorityComparer = new EnemyPriorityComparer();
        #endregion
        #region Class Body
        void Awake()
        {
            m_onSpawn?.Init();
            m_clock.OnTimeElapsed += OnTimeElapsedUpdate;
        }
        #region Spawning
        // When a spawner requests an enemy spawn
        public void SpawnEnemy(EventInfo info)
        {
            if (info is EnemySpawnInfo spawn)
            {
                SpawnEnemy_Internal(spawn.Behaviour, spawn.SpawnWorldPosition, spawn.Parent);
            }
        }
        protected void SpawnEnemy_Internal(PoolableBehaviour behaviour, Vector3 position, Transform parent = null)
        {
            if (!(behaviour is IEnemy))
            {
                return;
            }
            // Look for available pooled instances
            PoolableBehaviour newBehaviour = m_instance.GetInstanceFromAsset(behaviour.gameObject, parent);
            // setup new spawn instance
            newBehaviour.gameObject.transform.position = position;
            newBehaviour.TryGetComponent(out IEnemy spawn);
            spawn.OnDefeat += OnEnemyRemove;
            spawn.OnReveal += OnEnemyReveal;
            spawn.OnHide += OnEnemyHide;
            m_toAdd.Add(spawn);
            // Update spawned enemy activeness according to time of day
            if(spawn is IOrganicLife life) 
            {
                UpdateActiveness(life);
            }
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
        void OnEnemyRemove(IEnemy defeated)
        {
            defeated.OnDefeat -= OnEnemyRemove;
            defeated.OnReveal -= OnEnemyReveal;
            defeated.OnHide -= OnEnemyHide;
            m_toRemove.Add(defeated);
        }
        #endregion
        // Whenever player spends time, update all enemies with countdowns 
        // returns: whether there are Responses from active enemies
        // out: enemy responses
        public bool OnPlayerAction(int timeSpent, out List<Action> resp)
        {
            // Update all enemy countdowns here and get all responses
            resp = UpdateActiveEnemies(timeSpent);
            return resp.Count > 0;
        }

        void OnTimeElapsedUpdate(int dayCount, int hour, GameClock.TimeOfDay timeOfDay)
        {
            foreach (IEnemy e in m_activeEnemies)
            {
                if (e is IOrganicLife life)
                {
                    UpdateActiveness(life);
                }
            }
        }
        void UpdateActiveness(IOrganicLife life)
        {
            ActiveTimeFrame activeHours = life.ActiveHours;
            bool withActiveHours = activeHours.ActiveAt == m_clock.CurrentTimeOfDay;
            if (withActiveHours && !life.IsActive) 
            {
                life.Activate();
            }
            else if(!withActiveHours && life.IsActive) 
            {
                life.Hibernate();
            }
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
                e?.OnDefeated();
            }
            m_toAdd.Clear();
            m_toRemove.Clear();
        }

        // Call all active enemies to respond to player action
        List<Action> UpdateActiveEnemies(int dt) 
        {
            // make sure list is up to date before and after
            UpdateActivity();
            List<IEnemy> executeOrder = new List<IEnemy>();
            foreach (IEnemy enemy in m_activeEnemies)
            {
                // returns true if countdown reached, add to execution list
                if (enemy.UpdateCountdown(dt)) 
                {
                    executeOrder.Add(enemy);
                }
            }
            // sort execution order by ascending countdown value
            executeOrder.Sort(m_priorityComparer);
            List<Action> calls = new List<Action>();
            foreach (IEnemy e in executeOrder)
            {
                calls.Add(e.ExecuteAction);
            }
            UpdateActivity();
            return calls;
        }
        #endregion
    }
}