using System;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

namespace Curry.Ai
{
    [RequireComponent(typeof(Seeker))]
    [AddComponentMenu("Curry/Pathfinding/AIPath (2D)")]
    public class BaseNpcPathAi : AIPath, IAstarAI, IPathAi
    {
        protected override void OnEnable()
        {
            canMove = false;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            canMove = false;
            base.OnDisable();
        }

        public override void OnTargetReached()
        {
            canMove = false;
        }
        protected override void OnPathComplete(Path newPath)
        {
            if (newPath != null && !newPath.error) 
            {
                canMove = true;
                base.OnPathComplete(newPath);
            }
        }
        public void PlanPath(Transform target)
        {
            destination = target.position;
            SearchPath();
        }

        public void PlanPath(Vector3 target)
        {
            destination = target;
            SearchPath();
        }

        public void Wander()
        {
            throw new NotImplementedException();
        }
    }
}
