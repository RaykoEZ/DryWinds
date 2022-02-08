using System;
using UnityEngine;
using Pathfinding;

namespace Curry.Ai
{
    [RequireComponent(typeof(Seeker))]
    [AddComponentMenu("Curry/Pathfinding/AIPath (2D)")]
    public class BaseNpcPathAi : AIPath, IAstarAI, IPathAi
    {
        public virtual bool MovementFinished { get { return reachedDestination; } }
        protected float WanderDistance { get { return UnityEngine.Random.Range(0.8f, 1.2f) * 5f; } }

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
            if (!newPath.error) 
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
            Vector2 randDir = GetRandomDirection();
            Vector2 randPos = GetRandomDestination(position, randDir);
            Vector3 target = new Vector3(randPos.x, randPos.y, position.z);
            PlanPath(target);
        }
        protected virtual Vector2 GetRandomDirection()
        {
            float randDirX = UnityEngine.Random.Range(-1f, 1f);
            float randDirY = UnityEngine.Random.Range(-1f, 1f);
            Vector2 dir = new Vector2(randDirX, randDirY);
            return dir.normalized;
        }
        protected virtual Vector2 GetRandomDestination(Vector2 origin, Vector2 dir)
        {
            float randDegree = UnityEngine.Random.Range(-180f, 180f);
            Vector2 randRot = Quaternion.AngleAxis(randDegree, Vector3.forward) * dir;
            Vector2 future = origin + WanderDistance * randRot;
            return future;
        }

        public void InterruptPath()
        {
            ClearPath();
        }
    }
}
