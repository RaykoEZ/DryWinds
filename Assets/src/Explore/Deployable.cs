using UnityEngine;
using UnityEngine.AI;
using Curry.Game;

namespace Curry.Explore
{
    public class Deployable : Explorer, IPathExplorer
    {
        public int Id = 0;
        public DeploymentResource DeployCost = default;
        public string Name = default;
        public string Description = default;
        protected float m_timer = 0f;
        protected virtual void FixedUpdate() 
        {
            m_timer += Time.deltaTime;
            if(m_timer > 3f) 
            {
                ActivateEffect();
            }
        
        }
        
        public virtual void Deploy(Vector2 pos) 
        {

            OnDeploy();
        }

        protected virtual void OnDeploy() 
        { 
        
        }

        protected virtual void ActivateEffect() 
        { 
            
        }

        protected void SetDestination(Vector2 dest) 
        {
            m_agent.SetDestination(dest);
            m_agent.isStopped = false;
        }

    }

}
