using System;
using UnityEngine;

namespace Curry.Game
{
    public class AiPlanner 
    { 
        
    }

    public abstract class AiStateMachine : MonoBehaviour 
    {
        [SerializeField] protected NpcController m_controller = default;
        [SerializeField] protected AiState m_idle = default;

        public event OnAiStateTransition OnTransition;
        protected AiState m_current;
        protected AiState m_previous;

        protected virtual void Start() 
        {
            TransitionTo(m_idle);
        }

        protected virtual void Update() 
        {
            m_current.OnUpdate(m_controller);
            Evaluate();
        }

        // Determine state changes or additional behaviour
        protected abstract void Evaluate();

        protected virtual void TransitionTo(AiState next)
        {
            m_previous = m_current;
            m_current.TransitionTo(next);
        }

        protected virtual void OnTransitionFinished(AiState next)
        {
            if(m_previous != null) 
            {
                m_previous.OnTransition -= OnTransitionFinished;
            }
            OnTransition?.Invoke(next);

            m_current = next;
            m_current.OnTransition += OnTransitionFinished;
            m_current.OnEnter(m_controller);
        }
    }

}
