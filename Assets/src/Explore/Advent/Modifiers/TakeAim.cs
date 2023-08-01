using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public interface IEnemyReaction 
    {
        event OnAbilityMessage OnMessage;
        bool CanReact(IEnemy user);
        AbilityContent AbilityDetail { get; }
        public IEnumerator OnPlayerAction(IEnemy enemy);
    }
    [Serializable]
    public class TakeAim : StackableModifier, IAttackModifier, IStackableEffect
    {
        [SerializeField] protected int m_damageUpPerStack;
        protected override string ValuePerStackDisplay => $"{m_damageUpPerStack} Damage Up";
        public TakeAim(TakeAim toCopy) : base(toCopy)
        {
            m_damageUpPerStack = toCopy.m_damageUpPerStack;
        }
        public int Apply(int hitVal)
        {
            Trigger();
            int stack = m_currentStack;
            ResetStack();
            return hitVal + (stack * m_damageUpPerStack);
        }
        protected override TacticalStats Process_Internal(TacticalStats baseVal)
        {
            return baseVal;
        }
    }
}
