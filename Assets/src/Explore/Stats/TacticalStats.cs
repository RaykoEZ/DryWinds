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
        [Range(0, 3)]
        [SerializeField] int moveRange;
        [SerializeField] int speed;

        public TacticalStats(int maxHP, int hpIn, int range, int spd)
        {
            maxHp = Mathf.Max(maxHP, 0);
            hp = Mathf.Max(hpIn, 0);
            moveRange = Mathf.Max(range, 1);
            speed = Mathf.Max(spd, 0);
        }

        public int MaxHp { get => maxHp; set => maxHp = value; }
        public int Hp { get => hp; set => hp = value; }
        public int MoveRange { get => moveRange; set => moveRange = Mathf.Max(value, 1); }
        public int Speed { get => speed; set => speed = value; }
        public static TacticalStats operator +(TacticalStats a) => a;
        public static TacticalStats operator +(TacticalStats a, TacticalStats b) => 
            new TacticalStats(a.maxHp + b.maxHp, a.hp + b.hp, a.moveRange + b.moveRange,a.speed + b.speed);
        public static TacticalStats operator -(TacticalStats a, TacticalStats b) =>
            new TacticalStats(a.maxHp - b.maxHp, a.hp - b.hp, a.moveRange - b.moveRange, a.speed - b.speed);
        public static TacticalStats operator *(TacticalStats a, TacticalStats b) =>
            new TacticalStats(a.maxHp * b.maxHp, a.hp * b.hp, a.moveRange * b.moveRange, a.speed * b.speed);
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
