using UnityEngine;

namespace Curry.Game
{
    public interface ICollectable : IProximityPrompt
    {
        GameObject CollectedObject { get; }
        void Use();
        void Discard();
    }
}
