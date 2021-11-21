using System;
using UnityEngine;

namespace Curry.Game
{
    public interface IClashable 
    {
        CollisionStats CollisionData { get; }
        void OnClash(Collision2D collision);
    }
}
