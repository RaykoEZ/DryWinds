using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Curry.Ai
{
    // A handler for Astar Pathfinding (by Aron Granberg) project's ai movement script
    [RequireComponent(typeof(IAstarAI))]
    [RequireComponent(typeof(Seeker))]
    public class BasePathfindingHandler : MonoBehaviour, IPathHandler
    {
        Coroutine m_follow = default;
        protected virtual IAstarAI PathingAi
        {
            get { return GetComponent<IAstarAI>(); }
        }
        protected virtual Seeker Seeker
        {
            get { return GetComponent<Seeker>(); }
        }

        public event OnPathPlanned OnPlanned;
        public event OnDestinationReached OnReached;

        protected virtual void OnEnable() 
        {
            PathingAi.canMove = false;
            Seeker.pathCallback += PlannerFinished;
        }

        protected virtual void OnDisable() 
        {
            Seeker.pathCallback -= PlannerFinished;
        }

        protected virtual void PlannerFinished(Path path) 
        {
            OnPlanned?.Invoke(!path.error);
        }
        protected virtual void DestinationReached()
        {
            PathingAi.canMove = false;
            OnReached?.Invoke();
        }

        public virtual void PlanPath(Transform target)
        {
            PathingAi.destination = target.position;
        }

        public virtual void PlanPath(Vector3 target)
        {
            PathingAi.destination = target;
        }

        public virtual void FollowPlannedPath()
        {
            if(m_follow == null) 
            {
                PathingAi.canMove = true;
                m_follow = StartCoroutine(OnFollowPath());
            }
        }

        protected virtual IEnumerator OnFollowPath() 
        {
            yield return new WaitUntil(()=> { return PathingAi.reachedDestination; });
            DestinationReached();
            m_follow = null;
        }
    }
}
