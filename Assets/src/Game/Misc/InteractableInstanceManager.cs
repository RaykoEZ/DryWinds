using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class InteractableInstanceManager : MonoBehaviour 
    { 
        [SerializeField] InteractablePoolCollection m_poolManager = default;
        ObjectPool<Interactable> m_currentPool = default;

        public virtual void PrepareNewInstance(GameObject instanceAsset, Transform parent = null)
        {
            string assetId = instanceAsset.name;
            if (m_poolManager.ContainsPool(assetId))
            {
                m_currentPool = m_poolManager.GetPool(assetId);
            }
            else if (instanceAsset.TryGetComponent(out Interactable reference))
            {
                m_currentPool = m_poolManager.AddPool(assetId, reference, parent);
            }
        }

        public virtual Interactable GetInstanceFromAsset(GameObject asset)
        {
            PrepareNewInstance(asset);
            return m_currentPool?.GetItemFromPool();
        }

        public virtual Interactable GetInstanceFromCurrentPool() 
        {
            return m_currentPool?.GetItemFromPool();
        }
    }
}
