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

        public virtual bool TargetReached { get { return PathingAi.reachedDestination; } }

        protected virtual IAstarAI PathingAi
        {
            get { return GetComponent<IAstarAI>(); }
        }
        protected virtual Seeker Seeker
        {
            get { return GetComponent<Seeker>(); }
        }

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
            Debug.Log($"Log: {path.errorLog}");
            if (!path.error) 
            {
                FollowPlannedPath();
            }
        }
        protected virtual void DestinationReached()
        {
            PathingAi.canMove = false;
        }

        public virtual void PlanPath(Transform target)
        {
            PathingAi.destination = target.position;
            PathingAi.SearchPath();
        }

        public virtual void PlanPath(Vector3 target)
        {
            PathingAi.destination = target;
            PathingAi.SearchPath();
        }

        protected virtual void FollowPlannedPath()
        {
            if (m_follow != null) 
            {
                StopCoroutine(m_follow);
            }
            m_follow = StartCoroutine(OnFollowPath());
        }

        protected virtual IEnumerator OnFollowPath() 
        {
            PathingAi.canMove = true;
            yield return new WaitUntil(() => 
            { 
                return PathingAi.reachedDestination; 
            });
            DestinationReached();
            m_follow = null;
        }
    }
}
