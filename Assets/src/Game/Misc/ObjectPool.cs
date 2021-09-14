using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{    
    // Instantiates and enable/disables a type of object when notified
    public class ObjectPool<T> : IObjectPool<T> where T: MonoBehaviour, IPoolable
    {
        protected int m_amountToPool = default;
        protected T m_poolObjectRef = default;
        protected Transform m_parent = default;
        protected Queue<T> m_pool = new Queue<T>();
        protected List<T> m_inUse = new List<T>();
        public ObjectPool(int numToPool, T poolObj, Transform parent) 
        {
            m_amountToPool = numToPool;
            m_poolObjectRef = poolObj;
            m_parent = parent;
            if (m_amountToPool > 0 && m_poolObjectRef != null)
            {
                FillPool(m_amountToPool, m_poolObjectRef, m_parent);
            }
        }

        // Start is called before the first frame update
        protected virtual void FillPool(int numToPool, T objRef, Transform parent)
        {
            for(int i = 0; i < numToPool; ++i) 
            {
                MakePoolItem(objRef, parent);
            }
        }

        protected virtual T MakePoolItem(T objRef, Transform parent) 
        {
            T item = Object.Instantiate(objRef, parent);
            item.Prepare();
            item.Origin = this;
            item.gameObject.SetActive(false);
            m_pool.Enqueue(item);
            return item;
        }

        public virtual T GetItemFromPool()
        {
            T newObj;
            if (m_pool.Count == 0) 
            {
                newObj = MakePoolItem(m_poolObjectRef, m_parent);
            }
            else 
            {
                newObj = m_pool.Dequeue();
            }

            newObj.gameObject.SetActive(true);
            m_inUse.Add(newObj);
            // no more available, make new into pool
            return newObj;
        }

        public virtual void ReturnToPool(T obj) 
        {
            obj.gameObject.SetActive(false);
            if (m_inUse.Remove(obj)) 
            {
                m_pool.Enqueue(obj);
            }
        }

        public virtual void ReturnToPool(object instance)
        {
            // if instance is a poolable, return it to pool.
            if (instance is T)
            {
                ReturnToPool(instance as T);
            }
        }
    }
}



 
