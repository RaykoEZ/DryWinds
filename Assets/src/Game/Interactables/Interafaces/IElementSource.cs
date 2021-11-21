using UnityEngine;

namespace Curry.Game
{
    public interface IElementSource 
    { 
        float Strength { get; }
        Vector2 Origin { get; }
    }
}
