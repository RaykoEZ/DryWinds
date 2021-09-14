using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] int m_numToPool = default;
        [SerializeField] Transform m_parent = default;
        Dictionary<string, ObjectPool> m_pools = new Dictionary<string, ObjectPool>();

        public ObjectPool AddPool(string poolId, GameObject objRef) 
        {
            ObjectPool pool = new ObjectPool(m_numToPool, objRef, m_parent);
            m_pools.Add(poolId, pool);
            return pool;
        }

        public ObjectPool GetPool(string poolId) 
        {
            return m_pools[poolId];
        }

        public bool ContainsPool(string poolId) 
        {
            return m_pools.ContainsKey(poolId);
        }
    }
}
