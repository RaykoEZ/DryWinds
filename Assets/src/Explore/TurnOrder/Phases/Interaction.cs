using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using System.Collections;
using Curry.UI;
using Curry.Util;

namespace Curry.Explore
{
    // Handles player card/adventure plays
    // Handles AI actions and reactions
    public class Interaction : Phase
    {
        [SerializeField] EnemyManager m_enemy = default;
        [SerializeField] PlayManager m_play = default;
        Stack<List<IEnumerator>> m_interruptBuffer = new Stack<List<IEnumerator>>();

        protected override Type NextState => typeof(PlayerAction);

        public override void Init()
        {
            m_play.OnActivate += OnPlayerAction;
            m_enemy.OnActionBegin += OnEnemyAction;
        }
        void OnPlayerAction(int timeSpent, List<IEnumerator> actions = null) 
        {
            m_interruptBuffer?.Push(actions);
            // Check if there are enemy responses for this player action
            if (m_enemy.OnEnemyInterrupt(timeSpent, out List<IEnumerator> resp)) 
            {
                m_interruptBuffer?.Push(resp);
            }
            Interrupt();
        }
        void OnEnemyAction(List<IEnumerator> actions) 
        {
            m_interruptBuffer.Push(actions);
            Interrupt();
        }

        protected override IEnumerator Evaluate_Internal()
        {
            StartInterrupt();
            // If we need to resolve interrupts from activated enemies, do it first
            while (m_interruptBuffer.Count > 0)
            {
                foreach (IEnumerator call in m_interruptBuffer.Pop())
                {
                    yield return StartCoroutine(call);
                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds(0.5f);
            }
            TransitionTo();
            EndInterrupt();
        }
    }
}