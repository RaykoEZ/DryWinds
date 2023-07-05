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
        [SerializeField] int maxHp;
        [SerializeField] int hp;
        [Range(1, 3)]
        [SerializeField] int moveRange;
        [SerializeField] int speed;

        public int MaxHp { get => maxHp; set => maxHp = value; }
        public int Hp { get => hp; set => hp = value; }
        public int MoveRange { get => moveRange; set => moveRange = Mathf.Max(value, 1); }
        public int Speed { get => speed; set => speed = value; }
    }
    public interface ITimedElement<T>
    {
        void OnTimeElapsed(T dt);
    }
    public interface IMovementElement<T> 
    {
        void OnCharacterMoved(T baseStat);
    }
    public interface IExpiringElement
    {
        // A description of when the element expires, for displaying modifiers that expires when something happens
        string ExpiryCondition { get; }
    }
    public interface IDamageModifier 
    {
        int Apply(int hitVal);
    }
    public interface IAttackModifier
    {
        int Apply(int hitVal);
    }
}
