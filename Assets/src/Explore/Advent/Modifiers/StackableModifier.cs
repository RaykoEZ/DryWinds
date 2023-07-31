using UnityEngine;

namespace Curry.Explore
{
    public abstract class StackableModifier : TacticalModifier , IStackableEffect
    {
        [SerializeField] protected int m_maxStack = default;
        protected int m_currentStack = 0;
        public int MaxStack => m_maxStack; 
        public int CurrentStack => m_currentStack;
        // For displaying content, we need to have a value for each stack
        protected abstract string ValuePerStackDisplay { get; } 
        public override ModifierContent Content => new ModifierContent
        {
            Icon = m_resource.Content.Icon,
            Name = $"{m_resource.Content.Name} (x {m_currentStack})",
            Description = $"{m_resource.Content.Description} ({m_currentStack} x {ValuePerStackDisplay} total, x{MaxStack} max)."
        };
        public StackableModifier(StackableModifier toCopy) : base(toCopy)
        {
            m_maxStack = toCopy.m_maxStack;
            m_currentStack = toCopy.m_currentStack;
        }
        public virtual void AddStack(int addVal = 1)
        {
            int result = m_currentStack + addVal;
            m_currentStack = Mathf.Clamp(result, 0, m_maxStack);
        }
        public virtual void ResetStack()
        {
            m_currentStack = 0;
        }
        public virtual void SubtractStack(int subVal = 1)
        {
            int result = m_currentStack - subVal;
            m_currentStack = Mathf.Clamp(result, 0, m_currentStack);
        }
    }
}
