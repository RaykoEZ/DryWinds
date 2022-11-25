using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
namespace Curry.Explore
{
    public class EnemyAction : Phase
    {
        [SerializeField] TacticalEnemyManager m_enemy = default;
        Stack<List<Action>> m_interruptBuffer = new Stack<List<Action>>();
        public override void Init()
        {
            NextState = typeof(TurnEnd);
            m_enemy.OnEnemyInterrupt += OnEnemyInterrupting;
        }
        void OnEnemyInterrupting(List<Action> interrupt) 
        {
            // push interrupts to call after interrupting
            m_interruptBuffer.Push(interrupt);
            // Request interrupt
            Interrupt();
        }
        protected override void Evaluate()
        {
            if( m_interruptBuffer != null) 
            {
                // If we need to resolve interrupts from activated enemies, do it first
                while (m_interruptBuffer.Count > 0)
                {
                    foreach (Action call in m_interruptBuffer.Pop())
                    {
                        call?.Invoke();
                    }
                }
            }

            // Main process for all standby enemies
            m_enemy.OnPhaseBegin();
            TransitionTo();
        }
    }
}