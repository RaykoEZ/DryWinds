using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public abstract class AiStateMachine : MonoBehaviour 
    {
        [SerializeField] protected SkillInventoryManager m_skills = default;
        [SerializeField] protected NpcController m_controller = default;
        [SerializeField] protected AiState m_idle = default;

        public event OnAiStateTransition OnTransition;
        protected AiState m_current;
        protected AiState m_incoming;
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
            m_incoming = next;
            m_previous = m_current;
            m_current.OnExit();
        }

        protected virtual void OnTransitionFinished()
        {
            if(m_previous != null) 
            {
                m_previous.OnTransition -= OnTransitionFinished;
            }
            OnTransition?.Invoke();

            m_current = m_incoming;
            m_current.OnTransition += OnTransitionFinished;
            m_current.OnEnter(m_controller);
        }
    }

}
