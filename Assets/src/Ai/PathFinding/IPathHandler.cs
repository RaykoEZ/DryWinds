using System;
using UnityEngine;

namespace Curry.Ai
{
    public interface IPathHandler
    {
        bool TargetReached { get; }
        // Setting a target to plan a path
        void PlanPath(Transform target);
        void PlanPath(Vector3 target);
    }
}
