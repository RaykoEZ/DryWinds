using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public abstract class AiStateMachine : MonoBehaviour 
    {
        [SerializeField] protected NpcController m_controller = default;
        [SerializeField] protected AiAction m_idle = default;
        [SerializeField] protected List<AiAction> m_otherStates = default;

        public event OnAiActionTransition OnTransition;
        protected AiAction m_current;
        protected AiAction m_previous;
        bool m_transitionInProgress = false;

        protected virtual NpcWorldState WorldState
        {
            get
            {
                BaseNpc npc = m_controller.Character;
                return new NpcWorldState(
                    npc.CurrentStats,
                    npc.Enemies,
                    npc.Allies,
                    npc.BasicSkills.Skills,
                    npc.DrawSkills.Skills);
            }
        }

        protected virtual void Start() 
        {
            TransitionTo(m_idle);
        }

        protected virtual void Update() 
        {
            if (!m_transitionInProgress) 
            {
            }
            EvaluateGoal();
        }

        // Determine state changes or additional behaviour
        protected abstract void EvaluateGoal();

        protected virtual void TransitionTo(AiAction next)
        {
            m_previous = m_current;
            m_transitionInProgress = true;
            m_current.TransitionTo(next);
        }

        protected virtual void OnTransitionFinished(AiAction next)
        {
            if(m_previous != null) 
            {
                m_previous.OnTransition -= OnTransitionFinished;
                m_current.OnFinish -= EvaluateGoal;
            }
            OnTransition?.Invoke(next);
            m_transitionInProgress = false;
            m_current = next;
            m_current.OnTransition += OnTransitionFinished;
            m_current.OnFinish += EvaluateGoal;
            m_current.Execute(m_controller, WorldState);
        }
    }

}
