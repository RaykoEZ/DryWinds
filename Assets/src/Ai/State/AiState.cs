using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public class AiActionInput : IActionInput
    {
        public NpcController Controller { get; protected set; }
        public AiWorldState WorldState { get; protected set; }
        public Dictionary<string, object> Payload { get; protected set; }
        public AiActionInput(NpcController controller, AiWorldState state, Dictionary<string, object> payload = null)
        {
            Controller = controller;
            WorldState = state;
            Payload = payload;
        }
    }

    public delegate void OnAiStateEnd();
    [Serializable]
    public class AiState
    {
        [SerializeField] AiAction<IActionInput> m_action = default;
        public event OnAiStateEnd OnStateEnd;

        public virtual bool ActionInProgress { get { return m_action.OnCooldown; } }
        
        public virtual bool PreCondition(AiWorldState args)
        {
            return !ActionInProgress && m_action.IsUsable && m_action.PreCondition(args);
        }

        public virtual float Priority(AiWorldState args)
        {
            return m_action.Priority(args);
        }

        public virtual void OnEnter(NpcController controller, AiWorldState state) 
        {
            m_action.OnFinish += OnActionFinished;
            AiActionInput input = new AiActionInput(controller, state);
            m_action.OnEnter(input);
        }

        public virtual void OnTransition()
        {
            OnExit();
        }

        protected virtual void OnActionFinished(ICharacterAction<AiActionInput> acton) 
        {
            m_action.OnFinish -= OnActionFinished;
            OnStateEnd?.Invoke();
        }

        protected virtual void OnExit()
        {
            m_action.Interrupt();
        }
    }
}
