using UnityEngine;

namespace Curry.Game
{
    public interface ICollectable : IProximityPrompt
    { 
        string Name { get; }
        string Description { get; }
        Sprite ItemSprite { get; }
        GameObject CollectedObject { get; }
        void UseItem();
    }
}
