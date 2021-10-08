using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public interface ITargetable<T>
    { 
        T Value { get; }
    }

    public class TargetCharacter : ITargetable<BaseCharacter>
    {
        public BaseCharacter Value { get; private set; }
        public TargetCharacter(BaseCharacter val)
        {
            Value = val;
        }
    }

    public class TargetPosition : ITargetable<Vector2>
    {
        public Vector2 Value { get; private set; }
        public TargetPosition(Vector2 val)
        {
            Value = val;
        }
    }
    public class TargetDirection : ITargetable<Vector2>
    {
        public Vector2 Value { get; private set; }

        public TargetDirection(Vector2 val)
        {
            Value = val;
        }

        public TargetDirection(Vector2 origin, Vector2 dest)
        {
            Value = (dest - origin).normalized;
        }
    }
}
