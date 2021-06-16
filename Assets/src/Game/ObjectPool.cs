using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{    
    // Instantiates and enable/disables a type of object when notified
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] protected int m_amountToPool = default;
        [SerializeField] GameObject m_poolObjectRef = default;
        [SerializeField] Transform m_parent = default;
        protected List<PoolItem> m_pool = new List<PoolItem>();

        void Start()
        {
            if(m_amountToPool > 0 && m_poolObjectRef != null) 
            {
                MakePool(m_amountToPool, m_poolObjectRef, m_parent);
            }
        }

        // Start is called before the first frame update
        public virtual void MakePool(int numToPool, GameObject objRef, Transform parent)
        {
            for(int i = 0; i < numToPool; ++i) 
            {
                MakePoolItem(objRef, parent);
            }
        }

        protected virtual PoolItem MakePoolItem(GameObject objRef, Transform parent) 
        {
            PoolItem item = new PoolItem(objRef, parent);
            m_pool.Add(item);
            return item;
        }

        public virtual GameObject GetItem()
        {
            foreach(PoolItem item in m_pool) 
            {
                if (!item.IsActive) 
                {
                    return item.Obj;
                }
            }

            // no more available, make new into pool
            PoolItem newItem = MakePoolItem(m_poolObjectRef, m_parent);
            return newItem.Obj;
        }
    }
}



 
