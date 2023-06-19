using System;
using UnityEngine;
using Curry.Util;

namespace Curry.Explore
{
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
        [Range(0, 1000)]
        [SerializeField] int m_timeCost;
        [SerializeField] int m_cooldown;
        // How much room a card takes in hand, hand has a holding capacity (int)
        [SerializeField] int m_holdingValue;
        [SerializeField] bool m_isInitiallyOnCooldown;
        public string Name => m_name;
        public string Description => m_description;
        public RangeMap TargetingRange => m_targetingRange;
        public int TimeCost => m_timeCost;
        public int Cooldown => m_cooldown;
        public int HoldingValue => Mathf.Max(0, m_holdingValue);
        public bool IsInitiallyOnCooldown => m_isInitiallyOnCooldown;
    }
}
