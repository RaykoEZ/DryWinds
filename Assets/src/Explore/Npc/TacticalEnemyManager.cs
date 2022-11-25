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
    public delegate void OnInterrupt(List<Action> interrupts);
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
        // When tme ticks, update all enemies with countdowns 
        [SerializeField] protected CurryGameEventListener m_onTimeTick = default;
        public event OnInterrupt OnEnemyInterrupt;
        List<TacticalEnemy> m_standby = new List<TacticalEnemy>();
        List<TacticalEnemy> m_activatedEnemies = new List<TacticalEnemy>();
        // from activated to standby
        HashSet<TacticalEnemy> m_deactivating = new HashSet<TacticalEnemy>();
        // rom standby to activated
        HashSet<TacticalEnemy> m_activating = new HashSet<TacticalEnemy>();
        // enemies about to trigger actions
        List<TacticalEnemy> m_executing = new List<TacticalEnemy>();
        int m_numDirty = 0;

        // Comparer for enemy priority
        EnemyPriorityComparer m_priorityComparer = new EnemyPriorityComparer();
        private void Awake()
        {
            m_onSpawn?.Init();
            m_onTimeTick?.Init();
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
            spawn.OnCountdownUpdate += OnCountdownUpdate;
            spawn.OnActivate += OnEnemyActivate;
            spawn.OnStandby += OnEnemyStandby;
            spawn.OnDefeat += OnEnemyDefeated;
            m_standby.Add(spawn);
        }
        
        // countdown updates whenever tme is spent 
        public void OnTimeSpent(EventInfo time)
        {
            if (time is TimeInfo spend)
            {
                // Update all enemy countdowns here
                UpdateActiveEnemies(spend.Time);
            }
        }
        public void OnPhaseBegin() 
        {
            UpdateEnemyLists();
            m_numDirty = m_standby.Count;
            foreach (TacticalEnemy enemy in m_standby)
            {
                enemy.StandbyBehaviour();
            }
        }
        void OnEnemyActivate(TacticalEnemy activated) 
        {
            m_activating.Add(activated);
        }
        void OnEnemyStandby(TacticalEnemy deactivated) 
        {
            m_deactivating.Add(deactivated);
        }
        void UpdateEnemyLists() 
        {
            // Remove double-switching enemies
            m_activating.ExceptWith(m_deactivating);
            UpdateActivatedList();
            UpdateStandbyList();
        }
        void UpdateActivatedList() 
        {
            foreach(TacticalEnemy e in m_activating) 
            {
                if (m_standby.Remove(e)) 
                {
                    m_activatedEnemies.Add(e);
                }
            }
            m_activating.Clear();
        }
        void UpdateStandbyList() 
        {
            foreach (TacticalEnemy e in m_deactivating)
            {
                if (m_activatedEnemies.Remove(e))
                {
                    m_standby.Add(e);
                }
            }
            m_deactivating.Clear();
        }

        void OnEnemyDefeated(TacticalEnemy defeated) 
        {
            m_standby.Remove(defeated);
            m_activatedEnemies.Remove(defeated);
            defeated?.ReturnToPool();
        }
        void OnCountdownUpdate(TacticalEnemy enemy) 
        {
            m_numDirty--;
            // If this update has an interruptingaction, push it to the stack
            if(enemy.Countdown <= 0) 
            {
                m_executing.Add(enemy);
            }
            // When all finished updating, send the call stack to execute
            // and update any changes to enemy lists
            if(m_numDirty <= 0) 
            {
                // Sort executing enemies by lowest countdown, ascendingly
                m_executing.Sort(m_priorityComparer);
                List<Action> calls = new List<Action>();
                foreach (TacticalEnemy e in m_executing)
                {
                    calls.Add(e.ExecuteCall);
                }
                OnEnemyInterrupt?.Invoke(calls);
                UpdateEnemyLists();
            }
        }
        void UpdateActiveEnemies(int dt) 
        {
            // Make sure we have a valid list of active enemies
            UpdateEnemyLists();
            m_numDirty = m_activatedEnemies.Count;
            int cd;
            foreach (TacticalEnemy enemy in m_activatedEnemies)
            {          
                cd = enemy.UpdateCountdown(dt);
            }
        }
        #endregion
    }


}