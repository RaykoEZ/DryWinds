using UnityEngine;

namespace Curry.Game
{
    public interface ICollectable
    { 
        string Name { get; }
        string Description { get; }
        GameObject CollectedObject { get; }
        void OnUse();
    }
}
