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
    public delegate void OnInterrupt(Stack<Action> interrupts);
    // Monitors all spawned enemies, triggers enemy action phases
    public class TacticalEnemyManager : MonoBehaviour 
    {
        [SerializeField] EnemyInstanceManager m_instance = default;
        [SerializeField] protected CurryGameEventListener m_onSpawn = default;
        // When tme ticks, update all enemies with countdowns 
        [SerializeField] protected CurryGameEventListener m_onTimeTick = default;
        public event OnInterrupt OnEnemyInterrupt;
        List<TacticalEnemy> m_standby = new List<TacticalEnemy>();
        List<TacticalEnemy> m_activatedEnemies = new List<TacticalEnemy>();
        HashSet<TacticalEnemy> m_deactivating = new HashSet<TacticalEnemy>();
        HashSet<TacticalEnemy> m_activating = new HashSet<TacticalEnemy>();
        static Stack<Action> m_executingCalls = new Stack<Action>();
        int m_numDirty = 0;
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
        void OnCountdownUpdate(int countdown, Action onInterrupt = null) 
        {
            m_numDirty--;
            // If this update has an interruptingaction, push it to the stack
            if(countdown <= 0 && onInterrupt != null) 
            {
                m_executingCalls.Push(onInterrupt);
            }
            // When all finished updating, send the call stack to execute
            // and update any changes to enemy lists
            if(m_numDirty <= 0) 
            {
                OnEnemyInterrupt?.Invoke(m_executingCalls);
                UpdateEnemyLists();
            }
        }
        void UpdateActiveEnemies(int dt) 
        {
            UpdateEnemyLists();
            m_numDirty = m_activatedEnemies.Count;
            int cd;
            foreach (TacticalEnemy enemy in m_activatedEnemies)
            {          
                cd = enemy.UpdateCountdown(dt);
            }
        }
    }
}