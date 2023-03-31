using System;
using UnityEngine;

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
        [Range(0, 3)]
        public int MoveRange;
        public int Speed;
        public ObjectVisibility Visibility;
    }
    public interface ITimedElement<T_Arg>
    {
        void OnTimeElapsed(T_Arg dt);
    }

    public interface IDamageModifier 
    {
        int Apply(int hitVal);
    }
}
