using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
            StopAllCoroutines();
            StartCoroutine(Evaluate_Internal());
        }
        void OnEnemyAction(List<IEnumerator> actions) 
        {
            m_interruptBuffer.Push(actions);
            StopAllCoroutines();
            StartCoroutine(Evaluate_Internal(false));
        }
        protected IEnumerator PlayerAction_Internal() 
        {
            UpdateCallback();
            while (m_characterActionStack.Count > 0) 
            {
                var currentAction = m_characterActionStack.Pop();
                yield return CallActions(currentAction.Actions);
                // check for any generated actions after invoking current action
                UpdateCallback();
                // Check if there are enemy responses for this player action
                if (m_enemy.UpdateEnemyAction(
                    currentAction.ResourceSpent, out List<IEnumerator> resp) &&
                    resp != null)
                {
                    m_interruptBuffer?.Push(resp);
                }
            }
        }
        void UpdateCallback()
        {
            foreach(var toAdd in m_actionsToAdd) 
            {
                m_characterActionStack.Push(toAdd);
            }
            m_actionsToAdd.Clear();
        }
        protected IEnumerator Evaluate_Internal(bool giveBackPlayControl = true)
        {
            StartInterrupt();
            UpdateCallback();
            if (m_characterActionStack.Count > 0)
            {
                // Player goes first
                yield return StartCoroutine(PlayerAction_Internal());
            }
            // If we need to resolve interrupts from activated enemies, do it
            while (m_interruptBuffer.Count > 0)
            {
                yield return StartCoroutine(CallActions(m_interruptBuffer.Pop()));
            }
            
            if (giveBackPlayControl) 
            {
                EndInterrupt();
            }
        }
        protected IEnumerator CallActions(List<IEnumerator> actions) 
        {
            foreach (IEnumerator call in actions)
            {
                if (call != null) 
                {
                    yield return StartCoroutine(call);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}