using UnityEngine;

namespace Curry.Ai
{
    public interface IPathAi
    {
        PathState State { get; }
        void Startup();
        void Stop();
        // Setting a target to plan a path
        void Wander();
        void Flee(NpcTerritory territory);
    }
}
