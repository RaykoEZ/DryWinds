using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Curry.Game;
namespace Curry.Explore
{
    public delegate void OnEnemyActionFinish();
    public delegate void OnEnemyActionStart(List<IEnumerator> actions);
    // Monitors all spawned enemies, triggers enemy action phases
    public partial class EnemyManager : MonoBehaviour
    {
        #region Serialize Fields & Members
        [SerializeField] MovementManager m_movement = default;
        [SerializeField] EnemySpawnHandler m_spawning = default;
        [SerializeField] FogOfWar m_fog = default;
        EnemyContainer m_enemies = new EnemyContainer();
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
        public void StopEnemyActions() 
        {
            m_enemies.UpdateActivity();
            foreach (var item in m_enemies.ActiveEnemies)
            {
                item.StopCombat();
            }
        }
        public void ResumeEnemyActions() 
        {
            m_enemies.UpdateActivity();
            foreach (var item in m_enemies.ActiveEnemies)
            {             
                item.StartCombat();
            }
        }
        // Whenever player spends time, update all enemies with countdowns
        // Updates enemy action choice after player ends turn
        // returns: whether there are Responses from active enemies
        // out: enemy responses
        public bool EnemyReaction(ActionCost resourceSpent, out List<IEnumerator> reactions)
        {
            // Update all enemy countdowns here and get all responses
            reactions = UpdateEnemyReaction(resourceSpent);
            return reactions.Count > 0;
        }
        // Call all active enemies to respond to player action
        List<IEnumerator> UpdateEnemyReaction(ActionCost dt) 
        {
            // make sure list is up to date before and after
            m_enemies.UpdateActivity();
            List<IEnumerator> calls = new List<IEnumerator>();
            // sort execution order by ascending countdown value
            m_enemies.SortActiveEnemyPriorities();
            foreach (IEnemy enemy in m_enemies.ActiveEnemies)
            {
                // returns true if countdown reached, add to execution list
                if (enemy.Reaction(dt, out EnemyIntent reaction) && 
                    reaction.Call != null && reaction != EnemyIntent.None)
                {
                    calls.Add(reaction.Call);
                }
            }
            m_enemies.UpdateActivity();
            return calls;
        }
        #endregion
    }
}