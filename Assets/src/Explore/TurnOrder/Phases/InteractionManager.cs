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
    public class InteractionManager : SceneInterruptBehaviour
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
        Stack<PlayerActionItem> m_characterActionStack = new Stack<PlayerActionItem>();
        List<PlayerActionItem> m_actionsToAdd = new List<PlayerActionItem>();
        bool m_inProgress = false;
        void Start()
        {
            m_cardPlay.OnActivate += OnPlayerAction;
            m_playerMovement.OnStart += OnPlayerAction;
            m_enemy.OnActionBegin += OnEnemyAction;
        }
        void OnPlayerAction(ActionCost spent, List<IEnumerator> actions = null)
        {
            if (actions != null) 
            {
                var newItem = new PlayerActionItem { ResourceSpent = spent, Actions = actions};
                m_actionsToAdd.Add(newItem);
            }
            if (!m_inProgress) 
            {
                m_inProgress = true;
                StartCoroutine(Evaluate_Internal());
            }
        }
        void OnEnemyAction(List<IEnumerator> actions) 
        {
            m_interruptBuffer.Push(actions);
            if (!m_inProgress)
            {
                m_inProgress = true;
                StartCoroutine(Evaluate_Internal());
            }
        }
        protected IEnumerator PlayerAction_Internal() 
        {
            while(m_characterActionStack.Count > 0) 
            {
                var currentAction = m_characterActionStack.Pop();
                yield return CallActions(currentAction.Actions);
                yield return new WaitForEndOfFrame();
                // check for any generated actions after invoking current action
                UpdateCalltack();
                // Check if there are enemy responses for this player action
                if (m_enemy.OnEnemyInterrupt(
                    currentAction.ResourceSpent, out List<IEnumerator> resp) &&
                    resp != null)
                {
                    m_interruptBuffer?.Push(resp);
                }
            }

        }
        void UpdateCalltack()
        {
            foreach(var toAdd in m_actionsToAdd) 
            {
                m_characterActionStack.Push(toAdd);
            }
            m_actionsToAdd.Clear();
        }
        protected IEnumerator Evaluate_Internal()
        {
            StartInterrupt();
            UpdateCalltack();
            if (m_characterActionStack.Count > 0)
            {
                // Player goes first
                yield return StartCoroutine(PlayerAction_Internal());
                yield return new WaitForEndOfFrame();
            }
            // If we need to resolve interrupts from activated enemies, do it first
            while (m_interruptBuffer.Count > 0)
            {
                yield return StartCoroutine(CallActions(m_interruptBuffer.Pop()));
                yield return new WaitForSeconds(0.1f);
            }
            m_inProgress = false;
            EndInterrupt();
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