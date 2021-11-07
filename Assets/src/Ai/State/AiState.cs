using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;

namespace Curry.Ai
{
    [Serializable]
    public class AiState
    {
        [SerializeReference] AiAction<IActionInput, SkillProperty> m_action = default;
        protected virtual ICharacterAction<IActionInput, SkillProperty> ExecutingAction { get; set; }
        public bool ActionInProgress { get { return ExecutingAction != null && ExecutingAction.ActionInProgress; } }
        public virtual bool PreCondition(NpcWorldState args)
        {
            return m_action.PreCondition(args);
        }

        public virtual float Priority(NpcWorldState args)
        {
            return m_action.Priority(args);
        }

        public virtual void Execute(NpcController controller, NpcWorldState state) 
        {
            ExecutingAction = m_action.Execute(controller, state);
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
