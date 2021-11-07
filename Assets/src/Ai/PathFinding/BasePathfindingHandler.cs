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

        public event OnPathPlanned OnPlanned;

        protected virtual void OnEnable() 
        {
            PathingAi.canMove = false;
            PathingAi.canSearch = false;
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
            PathingAi.canSearch = false;
        }

        public virtual void PlanPath(Transform target)
        {
            PathingAi.canSearch = true;
            StartCoroutine(OnFollowTarget(target));
        }

        public virtual void PlanPath(Vector3 target)
        {
            PathingAi.canSearch = true;
            PathingAi.destination = target;
        }

        public virtual void FollowPlannedPath()
        {
            if(m_follow != null) 
            {
                StopCoroutine(m_follow);
            }
            PathingAi.canMove = true;
            m_follow = StartCoroutine(OnFollowPath());
        }

        protected virtual IEnumerator OnFollowPath() 
        {
            yield return new WaitUntil(()=> { return PathingAi.reachedDestination; });
            DestinationReached();
            m_follow = null;
        }

        protected virtual IEnumerator OnFollowTarget(Transform target)
        {
            while (!PathingAi.reachedDestination) 
            {
                PathingAi.destination = target.position;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
