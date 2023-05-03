using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public interface IEnemyReaction 
    {
        public void OnPlayerAction(IEnemy enemy);
    }

    [Serializable]
    public class Prepared : TacticalModifier, IAttackModifier, IStackableEffect
    {
        [Range(1, 3)]
        [SerializeField] protected int m_maxStack = default;
        [Range(1, 3)]
        [SerializeField] protected int m_currentStack = default;
        [SerializeField] protected int m_value = default;
        public int MaxStack => m_maxStack;
        public int CurrentStack => m_currentStack;
        public override ModifierContent Content => new ModifierContent 
        { 
            Icon = m_modIcon,
            Name = $"Prepared (x {m_currentStack})",
            Description = $"Attack Damage + {m_currentStack * m_value} (x {m_currentStack} stacks, max. {MaxStack} times)."
        };
        public Prepared(Prepared copy) : base(copy)
        {
            m_maxStack = copy.m_maxStack;
            m_value = copy.m_value;
            m_currentStack = copy.m_currentStack;
        }
        public int Apply(int hitVal)
        {
            Debug.Log("Prepared Spent x " + CurrentStack);
            Trigger();
            int stack = m_currentStack;
            ResetStack();
            return hitVal + (stack * m_value);
        }
        public void AddStack(int addVal = 1)
        {
            int result = m_currentStack + addVal;
            m_currentStack = Mathf.Clamp(result, 0, m_maxStack);
            Debug.Log("Prepared Stack x " + CurrentStack);
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
