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

        void OnEnemyActivate(TacticalEnemy activatedEnemy) 
        {
            if (m_standby.Remove(activatedEnemy))
            {
                m_activatedEnemies.Add(activatedEnemy);
            }
        }
        void OnEnemyStandby(TacticalEnemy deactivated) 
        {
            if (m_activatedEnemies.Remove(deactivated)) 
            {
                m_standby.Add(deactivated);
            }
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
            if(countdown <= 0 && onInterrupt != null) 
            {
                m_executingCalls.Push(onInterrupt);
            }
            // When all finished updating...
            if(m_numDirty <= 0) 
            {
                OnEnemyInterrupt?.Invoke(m_executingCalls);
            }
        }
        void UpdateActiveEnemies(int dt) 
        {
            m_numDirty = m_activatedEnemies.Count;
            foreach (TacticalEnemy enemy in m_activatedEnemies) 
            {          
                StartCoroutine(enemy.CountdownTick(dt));
            }
        }
    }
}