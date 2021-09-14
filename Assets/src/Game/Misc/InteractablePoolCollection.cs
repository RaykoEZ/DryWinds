using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class InteractablePoolCollection : MonoBehaviour
    {
        [SerializeField] int m_numToPool = default;
        [SerializeField] Transform m_defaultParent = default;
        Dictionary<string, ObjectPool<Interactable>> m_pools = new Dictionary<string, ObjectPool<Interactable>>();

        public ObjectPool<Interactable> AddPool(string poolId, Interactable objRef)
        {
            ObjectPool<Interactable> pool = new ObjectPool<Interactable>(m_numToPool, objRef, m_defaultParent);
            m_pools.Add(poolId, pool);
            return pool;
        }
        public ObjectPool<Interactable> AddPool(string poolId, Interactable objRef, Transform parent)
        {
            ObjectPool<Interactable> pool = new ObjectPool<Interactable>(m_numToPool, objRef, parent);
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
}
