using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public abstract class AiStateMachine : MonoBehaviour 
    {
        [SerializeField] protected NpcController m_controller = default;
        [SerializeField] protected AiState m_idle = default;

        public event OnAiStateTransition OnTransition;
        protected AiState m_current;
        protected AiState m_previous;
        bool m_transitionInProgress = false;
        protected virtual void Start() 
        {
            TransitionTo(m_idle);
        }

        protected virtual void Update() 
        {
            if (!m_transitionInProgress) 
            {
                NpcWorldState state = new NpcWorldState();
                m_current.OnUpdate(m_controller, state);
            }
            EvaluateGoal();
        }

        // Determine state changes or additional behaviour
        protected abstract void EvaluateGoal();

        protected virtual void TransitionTo(AiState next)
        {
            m_previous = m_current;
            m_transitionInProgress = true;
            m_current.TransitionTo(next);
        }

        protected virtual void OnTransitionFinished(AiState next)
        {
            if(m_previous != null) 
            {
                m_previous.OnTransition -= OnTransitionFinished;
            }
            OnTransition?.Invoke(next);

            m_transitionInProgress = false;
            m_current = next;
            m_current.OnTransition += OnTransitionFinished;
            NpcWorldState state = new NpcWorldState();

            m_current.OnEnter(m_controller, state);
        }
    }

}
