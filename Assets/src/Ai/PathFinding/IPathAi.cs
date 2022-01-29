using UnityEngine;

namespace Curry.Ai
{
    public delegate void OnPathComplete();
    public interface IPathAi
    {
        bool TargetReached { get; }
        // Setting a target to plan a path
        void PlanPath(Transform target);
        void PlanPath(Vector3 target);
        void Wander();
        void InterruptPath();
    }
}
