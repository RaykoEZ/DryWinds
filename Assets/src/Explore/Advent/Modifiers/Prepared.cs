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
    public class Prepared : StackableModifier, IAttackModifier, IStackableEffect
    {
        [SerializeField] protected int m_damageUpPerStack = default;
        public int DamageUpPerStack => m_damageUpPerStack = default;
        protected override string ValuePerStackDisplay => $"{m_damageUpPerStack} Damage Up";
        public Prepared(Prepared toCopy) : base(toCopy)
        {
            m_damageUpPerStack = toCopy.m_damageUpPerStack;
        }
        public int Apply(int hitVal)
        {
            Debug.Log("Prepared Spent x " + m_currentStack);
            Trigger();
            int stack = m_currentStack;
            ResetStack();
            return hitVal + (stack * DamageUpPerStack);
        }
        protected override TacticalStats Process_Internal(TacticalStats baseVal)
        {
            return baseVal;
        }
    }
}
