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
    public interface ITimedElement<T>
    {
        void OnTimeElapsed(T dt);
    }
    public interface IMovementElement<T> 
    {
        void OnCharacterMoved(T baseStat);
    }
    public interface IDamageModifier 
    {
        int Apply(int hitVal);
    }
}
