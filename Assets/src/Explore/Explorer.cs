using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Curry.Game;

namespace Curry.Explore
{
    public class Explorer : BaseCharacter, IPathExplorer
    {
        [SerializeField] NavMeshAgent m_agent = default;
        protected ExplorePath m_currentPath;
        protected Vector2 m_currentDest;
        protected Vector2 m_prevDest;
        protected Coroutine m_onMove;
        public ExplorePath CurrentPath { get { return m_currentPath; } protected set { m_currentPath = value; } }
        public Vector2 CurrentDestination { get { return m_currentDest; } protected set { m_currentDest = value; } }

        protected override void Awake()
        {
            m_agent.updateUpAxis = false;
            base.Awake();
        }

        public virtual void StopExploration()
        {
            m_agent.isStopped = true;
            Debug.Log("finished");
        }

        public void SetPath(ExplorePath path) 
        {
            m_currentPath = path; 
        }

        public virtual void StartExploration()
        {
            StopAllCoroutines();
            m_agent.isStopped = false;
            m_onMove = StartCoroutine(OnMove());
        }

        public virtual void OnDestinationReached()
        {
            m_prevDest = CurrentDestination;
        }

        protected virtual IEnumerator OnMove() 
        {
            while (!CurrentPath.Finished) 
            {
                CurrentDestination = CurrentPath.Destinations.Dequeue();
                m_agent.SetDestination(CurrentDestination);
                // Wait for set destination to prevent corner skipping
                yield return new WaitForSeconds(0.01f);
                yield return new WaitUntil(() => 
                { return m_agent.remainingDistance < m_agent.stoppingDistance; });
                OnDestinationReached();
            }
            StopExploration();        
        }
    }


    
}
