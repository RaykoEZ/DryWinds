using Curry.Game;
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
        public int MoveRange;
        public int Speed;
        public ObjectVisibility Visibility;
    }
    public abstract class TacticalModifier : IStatModifier<TacticalStats>
    {
        // Name of the modifier, e.g. a skill/item name
        [SerializeField] protected string m_name;
        public string Name => m_name;
        public event OnModifierExpire<TacticalStats> OnModifierExpire;
        public event OnModifierTrigger<TacticalStats> OnTrigger;
        public abstract TacticalStats Apply(TacticalStats baseVal);
        public abstract TacticalStats Revert(TacticalStats baseVal);
    }
}
