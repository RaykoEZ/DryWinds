using Curry.Game;
using UnityEngine;

namespace Curry.Explore
{
    public class MainExplorer : BaseCharacter
    {
        [SerializeField] ResourceManager m_resourceManager = default;

        public bool SpendResource(DeploymentResource cost) 
        {
            return m_resourceManager.SpendResource(cost);
        }
    }
}
