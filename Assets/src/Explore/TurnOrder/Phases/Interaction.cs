using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
namespace Curry.Explore
{
    // Handles player card/adventure plays
    // Handles AI actions and reactions
    public class Interaction : Phase
    {
        [SerializeField] EnemyManager m_enemy = default;
        [SerializeField] PlayManager m_play = default;
        Stack<List<Action>> m_interruptBuffer = new Stack<List<Action>>();
        public override void Init()
        {
            NextState = typeof(PlayerAction);
            m_play.OnActivate += OnPlayerAction;
        }
        void OnPlayerAction(int timeSpent, List<Action> actions = null) 
        {
            if (actions != null)
            {
                m_interruptBuffer.Push(actions);
            }
            // Check if there are enemy responses for this player action
            if (m_enemy.OnPlayerAction(timeSpent, out List<Action> resp)) 
            {
                m_interruptBuffer.Push(resp);
            }
            Interrupt();
        }
        protected override void Evaluate()
        {
            // If we need to resolve interrupts from activated enemies, do it first
            while (m_interruptBuffer.Count > 0)
            {
                foreach (Action call in m_interruptBuffer.Pop())
                {
                    call?.Invoke();
                }
            }       
            TransitionTo();
        }
    }
}