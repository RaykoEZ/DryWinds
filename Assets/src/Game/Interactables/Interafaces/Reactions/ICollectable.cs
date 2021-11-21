using UnityEngine;

namespace Curry.Game
{
    public interface ICollectable : IProximityPrompt
    {
        GameObject CollectedObject { get; }
        bool Activate();
        void Discard();
    }
}
