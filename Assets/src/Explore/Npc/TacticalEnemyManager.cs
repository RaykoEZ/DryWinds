using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    public class EnemyPoolCollection : PoolCollection<TacticalEnemy>
    {
    }
    [Serializable]
    public class EnemyInstanceManager : InstanceManager<TacticalEnemy>
    {
        [SerializeField] EnemyPoolCollection m_pool = default;
        protected override PoolCollection<TacticalEnemy> Pool { get { return m_pool; } }
    }
    // Monitors all spawned enemies, triggers enemy action phases
    public class TacticalEnemyManager : MonoBehaviour 
    {
        #region Comparer class
        // Comparer<TacticalEnemy> for priority sorting 
        protected class EnemyPriorityComparer : IComparer<TacticalEnemy>
        {
            public int Compare(TacticalEnemy x, TacticalEnemy y)
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
                        return x.Countdown - y.Countdown;
                    }
                }
            }
        }
        #endregion
        #region Class Body
        [SerializeField] EnemyInstanceManager m_instance = default;
        [SerializeField] protected CurryGameEventListener m_onSpawn = default;
        List<TacticalEnemy> m_activeEnemies = new List<TacticalEnemy>();
        HashSet<TacticalEnemy> m_toRemove = new HashSet<TacticalEnemy>();
        HashSet<TacticalEnemy> m_toAdd = new HashSet<TacticalEnemy>();

        // Comparer for enemy priority
        EnemyPriorityComparer m_priorityComparer = new EnemyPriorityComparer();
        private void Awake()
        {
            m_onSpawn?.Init();
        }
        public void SpawnEnemy(EventInfo info) 
        { 
            if (info is EnemySpawnInfo spawn) 
            {
                SpawnEnemy_Internal(spawn.SpawnRef, spawn.SpawnWorldPosition, spawn.Parent);
            }
        }
        protected void SpawnEnemy_Internal(TacticalEnemy enemy, Vector3 position, Transform parent = null) 
        {
            TacticalEnemy spawn = m_instance.GetInstanceFromAsset(enemy.gameObject, parent);
            spawn.gameObject.transform.position = position;
            spawn.OnDefeat += OnEnemyDefeated;
            m_toAdd.Add(spawn);
        }
        // Whenever player spends time, update all enemies with countdowns 
        // returns: whether there are Responses from active enemies
        // out: enemy responses
        public bool OnPlayerAction(int timeSpent, out List<Action> resp)
        {
            // Update all enemy countdowns here and get all responses
            resp = UpdateActiveEnemies(timeSpent);
            return resp.Count > 0;
        }

        void OnEnemyDefeated(TacticalEnemy defeated)
        {
            m_toRemove.Add(defeated);
            defeated?.ReturnToPool();
        }
        // add and remove scheduled updates to the enemy list
        void UpdateActivity() 
        {
            foreach (TacticalEnemy e in m_toAdd)
            {
                m_activeEnemies.Add(e);
            }
            foreach (TacticalEnemy e in m_toRemove) 
            {
                m_activeEnemies.Remove(e);
                e.ReturnToPool();
            }
            m_toAdd.Clear();
            m_toRemove.Clear();
        }
        List<Action> UpdateActiveEnemies(int dt) 
        {
            // make sure list is up to date before and after
            UpdateActivity();
            List<TacticalEnemy> executeOrder = new List<TacticalEnemy>();
            foreach (TacticalEnemy enemy in m_activeEnemies)
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
            foreach (TacticalEnemy e in executeOrder)
            {
                calls.Add(e.ExecuteAction);
            }
            UpdateActivity();
            return calls;
        }
        #endregion
    }
}