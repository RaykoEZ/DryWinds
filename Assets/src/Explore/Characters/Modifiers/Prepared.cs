using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Prepared : TacticalModifier, IAttackModifier, IStackableEffect
    {
        [Range(1, 3)]
        [SerializeField] protected int m_maxStack = default;
        [SerializeField] protected int m_value = default;
        protected int m_currentStack = 0;
        public int MaxStack => m_maxStack;
        public int CurrentStack => m_currentStack;
        public override ModifierContent Content => new ModifierContent 
        { 
            Icon = m_modIcon,
            Name = $"Prepared (x {m_currentStack})",
            Description = $"Attack Damage + {m_currentStack * m_value}"
        };
        public Prepared(Prepared copy) : base(copy)
        {
            m_maxStack = copy.m_maxStack;
            m_value = copy.m_value;
            m_currentStack = 0;
        }
        public int Apply(int hitVal)
        {
            Trigger();
            ResetStack();
            return hitVal + (m_currentStack * m_value);
        }
        public void AddStack(int addVal = 1)
        {
            int result = m_currentStack + addVal;
            m_currentStack = Mathf.Clamp(result, 0, m_maxStack);
        }
        public void SubtractStack(int subVal = 1)
        {
            int result = m_currentStack - subVal;
            m_currentStack = Mathf.Clamp(result, 0, m_maxStack);
        }
        public void ResetStack()
        {
            m_currentStack = 0;
        }
        protected override TacticalStats Apply_Internal(TacticalStats baseVal)
        {
            return baseVal;
        }
    }
}
