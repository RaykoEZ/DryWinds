using System.Collections;
using UnityEngine;
using Pathfinding;

namespace Curry.Ai
{
    public enum PathState
    {
        Idle,
        Wandering,
        Fleeing
    }

    [RequireComponent(typeof(Seeker))]
    [AddComponentMenu("Curry/Pathfinding/AIPath (2D)")]
    public class BaseNpcPathAi : AIPath, IAstarAI, IPathAi
    {
        protected float WanderDistance { get { return Random.Range(0.8f, 1.2f) * 5f; } }
        public PathState State { get { return m_pathState; } }
        protected PathState m_pathState = PathState.Idle;

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
            UpdatePathState();
        }

        protected override void OnPathComplete(Path newPath)
        {
            canMove = true;
            base.OnPathComplete(newPath);
        }

        public IEnumerator DelayMovement(float time) 
        {
            canMove = false;
            yield return new WaitForSeconds(time);
            canMove = true;
        }

        protected virtual void UpdatePathState() 
        {
            switch (m_pathState)
            {
                case PathState.Wandering:
                    Wander();
                    break;
                default:
                    m_pathState = PathState.Idle;
                    break;
            }
        }

        protected virtual void PlanPath(Vector3 target)
        {
            destination = target;
            SearchPath();
        }

        public virtual void Wander()
        {
            Vector2 randDir = GetRandomDirection();
            Vector2 randPos = GetRandomDestination(position, randDir);
            Vector3 target = new Vector3(randPos.x, randPos.y, position.z);
            m_pathState = PathState.Wandering;
            PlanPath(target);
        }

        public virtual void Flee(NpcTerritory territory)
        {
            Interrupt();
            Vector2 target = territory.transform.position;
            m_pathState = PathState.Fleeing;
            PlanPath(target);
        }

        protected virtual Vector2 GetRandomDirection()
        {
            float randDirX = Random.Range(-1f, 1f);
            float randDirY = Random.Range(-1f, 1f);
            Vector2 dir = new Vector2(randDirX, randDirY);
            return dir.normalized;
        }
        protected virtual Vector2 GetRandomDestination(Vector2 origin, Vector2 dir)
        {
            float randDegree = Random.Range(-180f, 180f);
            Vector2 randRot = Quaternion.AngleAxis(randDegree, Vector3.forward) * dir;
            Vector2 future = origin + WanderDistance * randRot;
            return future;
        }

        protected void Interrupt()
        {
            ClearPath();
        }
    }
}
