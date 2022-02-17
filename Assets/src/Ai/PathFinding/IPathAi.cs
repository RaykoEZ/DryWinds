using UnityEngine;

namespace Curry.Ai
{
    public delegate void OnTargetReached();
    public interface IPathAi
    {
        event OnTargetReached OnReached;
        PathState State { get; }
        void Startup();
        void Stop();
        void CancelCurrentPath();
        // Setting a target to plan a path
        void Wander();
        void Flee(NpcTerritory territory);
    }
}
