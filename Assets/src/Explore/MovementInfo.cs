using System.Collections.Generic;
using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    public class MovementInfo : EventInfo 
    { 
        public enum MovementDirection 
        { 
            Up,
            Down,
            Left,
            Right
        }

        public static Vector2 GetDirection(MovementDirection dir) 
        {
            Vector2 ret;
            switch (dir)
            {
                case MovementDirection.Up:
                    ret = Vector2.up;
                    break;
                case MovementDirection.Down:
                    ret = Vector2.down;
                    break;
                case MovementDirection.Left:
                    ret = Vector2.left;
                    break;
                case MovementDirection.Right:
                    ret = Vector2.right;
                    break;
                default:
                    ret = Vector2.zero;
                    break;
            }
            return ret;
        }
        public Vector2 Direction { get; protected set; }
        public int Magnitude { get; protected set; }
        public Vector3 ResultVector { get { return Magnitude * Direction; } }
        public MovementInfo SetDirection(MovementDirection dir) 
        {
            Direction = GetDirection(dir);
            return this;
        }

        public MovementInfo SetMagnitude(int v) 
        {
            Magnitude = v;
            return this;
        }
    }

    public class PositionInfo : EventInfo 
    {
        public Vector3 WorldPosition { get; protected set; }
        public PositionInfo(Vector3 pos) 
        {
            WorldPosition = pos;
        }
    }
    public class RangeInfo : EventInfo 
    {
        public List<Vector3> WorldPositions { get; protected set; }
        public RangeInfo(List<Vector3> rangeWorldPos) 
        {
            WorldPositions = rangeWorldPos;
        }
    }
}
