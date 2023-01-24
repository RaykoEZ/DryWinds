using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using Curry.Game;
using System;

namespace Curry.Explore
{
    public class SpawnInfo : EventInfo
    {
        public Vector3 SpawnWorldPosition { get; protected set; }
        public PoolableBehaviour Behaviour { get; protected set; }
        public Transform Parent { get; protected set; }
        // setup methods to call after object is instantiated
        public Action<PoolableBehaviour> OnInstantiate { get; protected set; }
        public SpawnInfo(Vector3 pos, PoolableBehaviour behaviour, Action<PoolableBehaviour> onInstance, Transform parent = null)
        {
            SpawnWorldPosition = pos;
            Behaviour = behaviour;
            OnInstantiate = onInstance;
            Parent = parent;
        }
    }
    [Serializable]
    // trigger event to spawn something into the scene
    public class SpawnHandler
    {
        [SerializeField] CurryGameEventTrigger m_spawnTrigger = default;
        public virtual void Spawn(Vector3 position, PoolableBehaviour spawnRef, Action<PoolableBehaviour> onInstance = null, Transform parent = null)
        {
            if (spawnRef == null) 
            {
                return;
            }
            SpawnInfo info = new SpawnInfo(position, spawnRef, onInstance, parent);
            m_spawnTrigger?.TriggerEvent(info);
        }
    }
}