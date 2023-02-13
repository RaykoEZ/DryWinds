using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class PoolCollection : MonoBehaviour
    {
        [SerializeField] int m_numToPool = default;
        [SerializeField] Transform m_defaultParent = default;
        Dictionary<string, ObjectPool<Interactable>> m_pools = new Dictionary<string, ObjectPool<Interactable>>();
        public ObjectPool<Interactable> AddPool(string poolId, Interactable objRef, Transform parent = null)
        {
            Transform transform = parent == null ? m_defaultParent : parent;
            ObjectPool<Interactable> pool = new ObjectPool<Interactable>(m_numToPool, objRef, transform);
            m_pools.Add(poolId, pool);
            return pool;
        }
        public ObjectPool<Interactable> GetPool(string poolId)
        {
            return m_pools[poolId];
        }

        public bool ContainsPool(string poolId) 
        {
            return m_pools.ContainsKey(poolId);
        }
    }

    public abstract class PoolCollection<T> where T : MonoBehaviour, IPoolable 
    {
        [SerializeField] int m_numToPool = default;
        [SerializeField] Transform m_defaultParent = default;
        Dictionary<string, ObjectPool<T>> m_pools = new Dictionary<string, ObjectPool<T>>();
        public Transform DefaultParent => m_defaultParent;

        public ObjectPool<T> AddPool(string poolId, T objRef, Transform parent = null)
        {
            Transform transform = parent == null ? m_defaultParent : parent;
            ObjectPool<T> pool = new ObjectPool<T>(m_numToPool, objRef, transform);
            m_pools.Add(poolId, pool);
            return pool;
        }
        public ObjectPool<T> GetPool(string poolId)
        {
            return m_pools[poolId];
        }

        public bool ContainsPool(string poolId)
        {
            return m_pools.ContainsKey(poolId);
        }
    }
}
