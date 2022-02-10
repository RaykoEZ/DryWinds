using UnityEngine;

namespace Curry.Ai
{
    public enum PathState
    {
        Idle,
        Wandering,
        Fleeing
    }
    public delegate void OnPathComplete();
    public interface IPathAi
    {
        PathState State { get; }
        bool MovementFinished { get; }
        // Setting a target to plan a path
        void Wander();
        void Flee(NpcTerritory territory);
        void Interrupt();
    }
}
