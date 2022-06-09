using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class InstanceManager : MonoBehaviour 
    { 
        [SerializeField] InteractablePoolCollection m_pool = default;
        ObjectPool<Interactable> m_currentPool = default;

        public virtual void PrepareNewInstance(GameObject instance, Transform parent = null)
        {
            string instanceId = instance.name;
            if (m_pool.ContainsPool(instanceId))
            {
                m_currentPool = m_pool.GetPool(instanceId);
            }
            else if (instance.TryGetComponent(out Interactable reference))
            {
                m_currentPool = m_pool.AddPool(instanceId, reference, parent);
            }
        }

        public virtual Interactable GetInstanceFromAsset(GameObject asset, Transform parent = null)
        {
            PrepareNewInstance(asset, parent);
            return m_currentPool?.GetItemFromPool();
        }
        public virtual Interactable GetInstanceFromCurrentPool() 
        {
            return m_currentPool?.GetItemFromPool();
        }
    }
}
