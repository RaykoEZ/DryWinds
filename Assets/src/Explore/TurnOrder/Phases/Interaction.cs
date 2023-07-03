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
        protected class PlayerActionItem 
        {
            public ActionCost ResourceSpent;
            public List<IEnumerator> Actions;
        }
        [SerializeField] MovementManager m_playerMovement = default;
        [SerializeField] EnemyManager m_enemy = default;
        [SerializeField] HandManager m_cardPlay = default;
        Stack<List<IEnumerator>> m_interruptBuffer = new Stack<List<IEnumerator>>();
        PlayerActionItem m_currentPlayerAction;
        protected override Type NextState { get; set; } = typeof(PlayerAction);

        public override void Init()
        {
            m_cardPlay.OnActivate += OnPlayerAction;
            m_playerMovement.OnStart += OnPlayerAction;
            m_enemy.OnActionBegin += OnEnemyAction;
        }
        void OnPlayerAction(ActionCost spent, List<IEnumerator> actions = null)
        {
            if (actions != null) 
            {
                m_currentPlayerAction = new PlayerActionItem { ResourceSpent = spent, Actions = actions};
                NextState = typeof(PlayerAction);
                Interrupt();
            }
        }
        void OnEnemyAction(List<IEnumerator> actions) 
        {
            m_interruptBuffer.Push(actions);
            NextState = typeof(EnemyAction);
            Interrupt();
        }
        protected IEnumerator PlayerAction_Internal() 
        {
            yield return CallActions(m_currentPlayerAction.Actions);
            yield return new WaitForEndOfFrame();
            // Check if there are enemy responses for this player action
            if (m_enemy.OnEnemyInterrupt(m_currentPlayerAction.ResourceSpent, out List<IEnumerator> resp))
            {
                m_interruptBuffer?.Push(resp);
            }                 
        }
        protected override IEnumerator Evaluate_Internal()
        {
            if (m_currentPlayerAction != null)
            {
                // Player goes first
                yield return StartCoroutine(PlayerAction_Internal());
                m_currentPlayerAction = null;
                yield return new WaitForEndOfFrame();
            }
            // If we need to resolve interrupts from activated enemies, do it first
            while (m_interruptBuffer.Count > 0)
            {
                yield return StartCoroutine(CallActions(m_interruptBuffer.Pop()));
                yield return new WaitForSeconds(0.1f);
            }
            TransitionTo();
        }
        protected IEnumerator CallActions(List<IEnumerator> actions) 
        {
            foreach (IEnumerator call in actions)
            {
                if (call != null) 
                {
                    yield return StartCoroutine(call);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}