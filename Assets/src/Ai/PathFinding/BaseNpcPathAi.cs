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
        protected float WanderDistance { get { return Random.Range(0.8f, 1.2f) * 3f; } }
        public PathState State { get { return m_pathState; } }
        protected PathState m_pathState = PathState.Idle;

        public event OnTargetReached OnReached;

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
            Stop();
            OnReached?.Invoke();
            UpdatePathState();
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
            Vector2 randPos = GetRandomDestination();
            Vector3 target = new Vector3(randPos.x, randPos.y, position.z);
            m_pathState = PathState.Wandering;
            PlanPath(target);
        }

        public virtual void Flee(NpcTerritory territory)
        {
            CancelCurrentPath();
            Vector2 target = territory.transform.position;
            m_pathState = PathState.Fleeing;
            PlanPath(target);
        }

        protected virtual Vector2 GetRandomDestination()
        {
            Vector2 randDir = Random.insideUnitCircle;
            Vector2 dest = randDir * WanderDistance;
            dest += (Vector2)position;
            var graph = AstarPath.active.graphs[0];
            var nearestNode = graph.GetNearest(dest);
            bool walkable = nearestNode.node.Walkable;
            if (!walkable) 
            {
                dest = Rotate(graph, dest, randDir);
            }
            return dest;
        }

        Vector2 Rotate(NavGraph graph, Vector2 dest, Vector2 dir) 
        {
            Vector2 o = position;
            // To allow more exploration in an enclosure, we reduce the wander distance after each unwalkable step
            float distMod = 0.8f;
            for (int i = 1; i < 7; ++i)
            {
                Vector2 rot = Quaternion.AngleAxis(i * 45f, Vector3.forward) * dir;
                // reducing wander dist to explore more within an enclosure
                dest = rot * distMod * WanderDistance;
                dest += o;
                var nearestNode = graph.GetNearest(dest);
                bool walkable = nearestNode.node.Walkable;
                if (walkable) 
                {
                    return dest;
                }
                else 
                {
                    distMod *= distMod;
                }
            }
            return dest;
        }

        public void CancelCurrentPath()
        {
            ClearPath();
        }

        public void Stop()
        {
            canMove = false;
            autoRepath.mode = AutoRepathPolicy.Mode.Never;
            CancelCurrentPath();
        }

        public void Startup()
        {
            canMove = true;
            autoRepath.mode = AutoRepathPolicy.Mode.Dynamic;
        }
    }
}
