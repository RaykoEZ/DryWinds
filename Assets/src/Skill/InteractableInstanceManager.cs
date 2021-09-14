using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class InteractableInstanceManager : MonoBehaviour 
    { 
        [SerializeField] InteractablePoolManager m_poolManager = default;
        ObjectPool<Interactable> m_currentPool = default;

        public virtual void PrepareNewInstance(Asset instanceAsset)
        {
            string assetId = instanceAsset.name;
            if (m_poolManager.ContainsPool(assetId))
            {
                m_currentPool = m_poolManager.GetPool(assetId);
            }
            else if(instanceAsset.Prefab.TryGetComponent(out Interactable reference))
            {
                m_currentPool = m_poolManager.AddPool(assetId, reference);
            }
        }
        public virtual Interactable GetInstanceFromAsset(Asset asset)
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
