using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public interface IPoolCollection
    {
        IObjectPool AddPool();
    }


    public class InteractablePoolCollection : MonoBehaviour
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
}
