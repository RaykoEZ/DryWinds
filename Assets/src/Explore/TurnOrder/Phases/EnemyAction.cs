using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
namespace Curry.Explore
{
    public class EnemyAction : Phase
    {
        [SerializeField] TacticalEnemyManager m_enemy = default;
        List<Action> m_interruptBuffer = new List<Action>();
        public override void Init()
        {
            NextState = typeof(TurnEnd);
            m_enemy.OnEnemyInterrupt += OnEnemyInterrupting;
        }
        void OnEnemyInterrupting(Stack<Action> interrupt) 
        {
            // store interrupting actions to call after interrupting
            m_interruptBuffer.AddRange(interrupt);
            // Request interrupt
            Interrupt();
        }
        protected override void Evaluate()
        {
            // If we need to resolve interrupts, do it first
            if(m_interruptBuffer != null && m_interruptBuffer.Count > 0) 
            {
                foreach(Action call in m_interruptBuffer) 
                {
                    call?.Invoke();
                }
                m_interruptBuffer.Clear();
            }
            // Main process
        }
    }
}