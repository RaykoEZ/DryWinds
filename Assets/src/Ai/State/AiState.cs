using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    public class AiState
    {
        [SerializeReference] AiAction<IActionInput> m_action = default;
        protected virtual ICharacterAction<IActionInput> ExecutingAction { get; set; }
        public virtual bool ActionInProgress { get { return ExecutingAction != null && ExecutingAction.ActionInProgress; } }
        public virtual bool PreCondition(AiWorldState args)
        {
            return m_action.PreCondition(args);
        }

        public virtual float Priority(AiWorldState args)
        {
            return m_action.Priority(args);
        }

        public virtual void Execute(NpcController controller, AiWorldState state) 
        {
            if (!ActionInProgress) 
            {
                ExecutingAction = m_action.Execute(controller, state);
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
