using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Curry.Game;
using Curry.UI;
namespace Curry.Explore
{
    public delegate void OnEnemyActionFinish();
    public delegate void OnEnemyActionStart(List<IEnumerator> actions);
    // Monitors all spawned enemies, triggers enemy action phases
    public partial class EnemyManager : SceneInterruptBehaviour
    {
        #region Serialize Fields & Members
        [SerializeField] MovementManager m_movement = default;
        [SerializeField] EnemySpawnHandler m_spawning = default;
        [SerializeField] FogOfWar m_fog = default;
        [SerializeField] DangerZonePreviewHanadler m_dangerZoneDisplay = default;
        EnemyContainer m_enemies = new EnemyContainer();
        // Comparer for enemy priority
        public event OnEnemyActionFinish OnActionFinish;
        public event OnEnemyActionStart OnActionBegin;
        #endregion
        #region Class Body
        void Awake()
        {
            m_spawning.SpawnProperties.OnSpawn?.Init();
        }
        void Start()
        {
            // Add all intially spawned enemies into the manager
            foreach(Transform t in m_spawning.SpawnProperties.InstanceManager.PoolDefaultParent) 
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
            if(info is not SpawnInfo) 
            {
                return;
            }
            var spawn = info as SpawnInfo;
            var result = m_spawning.SpawnEnemy_Internal(spawn.Behaviour, spawn.SpawnWorldPosition, spawn.OnInstantiate, spawn.Parent);
            if(result != null) 
            {
                InitInstance(result, result.transform.position);
            }
        }
        void InitInstance(PoolableBehaviour newBehaviour, Vector3 cellCenterWorld) 
        {
            // setup new spawn instance
            newBehaviour.gameObject.transform.position = cellCenterWorld;
            newBehaviour.TryGetComponent(out IEnemy spawn);
            if (spawn is IMovableEnemy movable) 
            {
                movable.OnMove += OnEnemyMovement;
                movable.OnBlocked += m_fog.OnMovementBlocked;
            }
            spawn.OnDefeat += OnEnemyRemove;
            spawn.OnReveal += m_fog.OnEnemyReveal;
            spawn.OnHide += m_fog.OnEnemyHide;
            m_enemies.ScheduleAdd(spawn);
        }
        void OnEnemyRemove(ICharacter remove)
        {
            if(remove is IMovableEnemy movable) 
            {
                movable.OnMove -= OnEnemyMovement;
                movable.OnBlocked -= m_fog.OnMovementBlocked;
            }
            remove.OnDefeat -= OnEnemyRemove;
            remove.OnReveal -= m_fog.OnEnemyReveal;
            remove.OnHide -= m_fog.OnEnemyHide;
            m_enemies.ScheduleRemove(remove as IEnemy);
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
        #endregion
        // Whenever player spends time, update all enemies with countdowns
        // Updates enemy action choice after player ends turn
        // returns: whether there are Responses from active enemies
        // out: enemy responses
        public bool UpdateEnemyAction(ActionCost resourceSpent, out List<IEnumerator> reactions)
        {
            // Update all enemy countdowns here and get all responses
            reactions = UpdateEnemyAction(resourceSpent);
            return reactions.Count > 0;
        }
        // Notifies call stack for enemy action calls
        public bool OnEnemyAction() 
        {
            List<IEnumerator> actions = GetCurrentEnemyAction();
            bool hasActions = actions.Count > 0;
            if (hasActions) 
            {
                OnActionBegin?.Invoke(actions);
            }
            return actions.Count > 0;
        }
        // Get current intended enemy actons
        List<IEnumerator> GetCurrentEnemyAction() 
        {
            List<IEnumerator> ret = new List<IEnumerator>();
            m_enemies.UpdateActivity();
            m_enemies.SortActiveEnemyPriorities();
            foreach (var item in m_enemies.ActiveEnemies)
            {
                EnemyIntent intent = item.IntendingAction;
                if(intent != null && intent.Call != null) 
                {
                    ret.Add(m_dangerZoneDisplay.ClearDanagerZone(item.GetTransform()));
                    ret.Add(intent.Call);
                }
            }
            if (ret.Count > 0)
            {
                ret.Add(FinishActionPhase());
            }
            m_enemies.UpdateActivity();
            return ret;
        }
        // Call all active enemies to respond to player action
        List<IEnumerator> UpdateEnemyAction(ActionCost dt) 
        {
            // make sure list is up to date before and after
            m_enemies.UpdateActivity();
            List<IEnumerator> calls = new List<IEnumerator>();
            // sort execution order by ascending countdown value
            m_enemies.SortActiveEnemyPriorities();
            foreach (IEnemy enemy in m_enemies.ActiveEnemies)
            {
                // returns true if countdown reached, add to execution list
                if (enemy.UpdateAction(dt, out EnemyIntent intent) && 
                    intent.Call != null)
                {
                    calls.Add(intent.Call);
                }
                m_dangerZoneDisplay.DisplayDangerZone(enemy.GetTransform(), enemy.IntendingAction.Ability);
            }
            if (calls.Count > 0) 
            {
                calls.Add(FinishActionPhase());
            }
            m_enemies.UpdateActivity();
            return calls;
        }
        IEnumerator FinishActionPhase() 
        {
            OnActionFinish?.Invoke();
            yield return new WaitForEndOfFrame();           
        }
        #endregion
    }
}