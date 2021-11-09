using System;
using UnityEngine;

namespace Curry.Game
{
    public interface IClashable 
    {
        CollisionStats CollisionData { get; }
        void OnClash(Collision2D collision);
    }

    public interface IElementSource 
    { 
        float Strength { get; }
        Vector2 Origin { get; }
    }

    public interface IFlammable
    {
        void OnTouchFire(IElementSource source);
    }

    public interface IPromptable 
    {
        void OnPrompt();
    }
}
