using System;
using UnityEngine;
using Curry.Util;

namespace Curry.Explore
{
    [Serializable]
    public struct ActionCost
    {
        [Range(0, 1000)]
        public int Time;
        [Range(0, 3)]
        public int ActionCount;
        public static ActionCost Free => new ActionCost { ActionCount = 0, Time = 0 }; 
    }
    public enum CardType
    {
        Skill = 0,
        Equipment = 1,
        Item = 2
    }
    // Basic information about a card
    [Serializable]
    public struct CardAttribute 
    {
        [SerializeField] string m_name;
        [TextArea]
        [SerializeField] string m_description;
        [SerializeField] RangeMap m_targetingRange;
        [SerializeField] ActionCost m_cost;
        [SerializeField] int m_cooldown;
        // How much room a card takes in hand, hand has a holding capacity (int)
        [SerializeField] int m_holdingValue;
        [SerializeField] bool m_isInitiallyOnCooldown;
        public string Name => m_name;
        public string Description => m_description;
        public RangeMap TargetingRange => m_targetingRange;
        public ActionCost Cost => m_cost;
        public int Cooldown => m_cooldown;
        public int HoldingValue => Mathf.Max(0, m_holdingValue);
        public bool IsInitiallyOnCooldown => m_isInitiallyOnCooldown;
    }
}
