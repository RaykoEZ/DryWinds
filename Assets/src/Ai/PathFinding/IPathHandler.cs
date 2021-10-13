using UnityEngine;

namespace Curry.Ai
{
    public delegate void OnPathPlanned(bool pathPossible);
    public delegate void OnDestinationReached();
    public interface IPathHandler
    {
        event OnPathPlanned OnPlanned;
        event OnDestinationReached OnReached;
        // Setting a target to plan a path
        void PlanPath(Transform target);
        void PlanPath(Vector3 target);
        // Move towards the planned path.
        void FollowPlannedPath();
    }


}
