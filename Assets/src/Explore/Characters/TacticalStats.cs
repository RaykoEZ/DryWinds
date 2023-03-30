using System;

namespace Curry.Explore
{
    [Serializable]
    public enum ObjectVisibility
    {
        Visible,
        Hidden
    }
    [Serializable]
    public struct TacticalStats 
    {
        public int MaxHp;
        public int Hp;
        public int MoveRange;
        public int Speed;
        public ObjectVisibility Visibility;
    }
    public interface ITimedElement<T_Arg>
    {
        void OnTimeElapsed(T_Arg dt);
    }
}
