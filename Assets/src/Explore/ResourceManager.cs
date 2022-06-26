using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] protected DeploymentResource InitResource = default;
        DeploymentResource m_currentResource;
        public DeploymentResource CurrentResource { get { return m_currentResource; } }

        void Awake()
        {
            Init();
        }

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
