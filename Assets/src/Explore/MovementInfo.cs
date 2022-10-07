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
        public Vector2 Direction { get; protected set; }
        public int Magnitude { get; protected set; }
        public Vector3 ResultVector { get { return Magnitude * Direction; } }
        public MovementInfo SetDirection(MovementDirection dir) 
        {
            switch (dir)
            {
                case MovementDirection.Up:
                    Direction = Vector2.up;
                    break;
                case MovementDirection.Down:
                    Direction = Vector2.down;
                    break;
                case MovementDirection.Left:
                    Direction = Vector2.left;
                    break;
                case MovementDirection.Right:
                    Direction = Vector2.right;
                    break;
                default:
                    break;
            }
            return this;
        }

        public MovementInfo SetMagnitude(int v) 
        {
            Magnitude = v;
            return this;
        }
    }
}
