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
        [SerializeField] protected string m_name = default;
        public override string ToString()
        {
            return m_name;
        }

        public virtual bool PreCondition(AiWorldState args)
        {
            return m_action.PreCondition(args);
        }

        public virtual float Priority(AiWorldState args)
        {
            return m_action.Priority(args);
        }

        public virtual void OnEnter(NpcController controller, AiWorldState state) 
        {
            AiActionInput input = new AiActionInput(controller, state);
            m_action.OnEnter(input);
        }
    }
}
