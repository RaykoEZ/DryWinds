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

    [Serializable]
    public class AiState
    {
        [SerializeField] AiAction<IActionInput> m_action = default;
        public virtual bool ActionInProgress { get { return m_action != null && m_action.ActionInProgress; } }
        
        public virtual bool PreCondition(AiWorldState args)
        {
            return m_action.PreCondition(args);
        }

        public virtual float Priority(AiWorldState args)
        {
            return m_action.Priority(args);
        }

        public virtual void ResolveState(NpcController controller, AiWorldState state) 
        {
            if (!ActionInProgress) 
            {
                AiActionInput input = new AiActionInput(controller, state);
                m_action.Execute(input);
            }
        }

        public virtual IEnumerator OnTransition(
            AiState next, 
            Action<AiState> onFinishCallback)
        {
            OnExit();
            yield return null;
            onFinishCallback?.Invoke(next);
        }

        protected virtual void OnExit()
        {
        }
    }
}
