using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    public class ResourceManager 
    {
        [SerializeField] protected DeploymentResource InitResource = default;
        DeploymentResource m_currentResource;
        public DeploymentResource CurrentResource { get { return m_currentResource; } }

        public void Init() 
        {
            m_currentResource = InitResource;
        }

        public bool SpendResource(DeploymentResource cost)
        {
            bool enoughResource = cost <= m_currentResource;
            if (enoughResource)
            {
                m_currentResource -= cost;
            }
            return enoughResource;
        }
    }
}
