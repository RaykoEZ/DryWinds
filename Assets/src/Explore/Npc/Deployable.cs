using UnityEngine;
using UnityEngine.AI;
using Curry.Game;

namespace Curry.Explore
{
    public abstract class Deployable : Explorer, IPathExplorer
    {
        public int Id = 0;
        public DeploymentResource DeployCost = default;
        public string Name = default;
        public string Description = default;
        
        public virtual void Deploy(Vector2 dest) 
        {
            OnDeploy();
            SetDestination(dest);
        }

        protected abstract void OnDeploy();

        protected abstract void ActivateAbility();

        protected void SetDestination(Vector2 dest) 
        {
            m_agent.SetDestination(dest);
            m_agent.isStopped = false;
        }

    }
}
