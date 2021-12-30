using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public class BaseAiStateMachine : MonoBehaviour 
    {
        [SerializeField] protected float m_defaultFrequency = default;
        [SerializeField] protected float m_averageReactionInterval = default;
        [SerializeField] protected NpcController m_controller = default;
        [SerializeField] protected AiState m_defaultAction = default;
        [SerializeField] protected List<AiState> m_otherActions = default;
        protected virtual float ReactionTime
        {
            get
            {
                return Random.Range(0.7f, 1.3f) * m_averageReactionInterval;
            }
        }

        protected virtual bool IsReady
        {
            get 
            {
                return m_controller.IsReady && !m_current.ActionInProgress;
            }
        }
        float m_evalTimer = 0f; 
        protected AiState m_current;
        AiWorldState m_worldStateSnapshot = new AiWorldState();
        protected virtual AiWorldState WorldStateSnapshot
        {
            get
            {
                return m_worldStateSnapshot;
            }
        }

        protected List<AiState> ValidStates
        {
            get
            {
                List<AiState> validActions = new List<AiState>();
                foreach (AiState state in m_otherActions)
                {
                    if (state.PreCondition(WorldStateSnapshot))
                    {
                        validActions.Add(state);
                    }
                }
                return validActions;
            }
        }

        protected virtual AiState BestState
        {
            get
            {
                List<AiState> states = ValidStates;
                AiState best = m_defaultAction;
                foreach (AiState state in states)
                {
                    if (state.Priority(WorldStateSnapshot) > best.Priority(WorldStateSnapshot))
                    {
                        best = state;
                    }
                }
                return best;
            }
        }

        protected virtual void Start() 
        {
            m_current = m_defaultAction;
            ExecuteCurrentAction();
        }

        protected virtual void Update() 
        {
            m_evalTimer += Time.deltaTime;
            if (m_evalTimer > m_defaultFrequency && IsReady) 
            {
               EvaluateActions();
               m_evalTimer = 0f;
            }
        }

        // Determine state changes or additional behaviour
        protected virtual void EvaluateActions() 
        {
            UpdateWorldState();
            StartCoroutine(Evaluate());
        }

        protected virtual IEnumerator Evaluate() 
        {
            yield return new WaitForSeconds(ReactionTime);
            TransitionTo(BestState);
        }

        protected virtual void TransitionTo(AiState next)
        {
            StartCoroutine(m_current.OnTransition(next, OnTransitionFinished));
        }

        protected virtual void OnTransitionFinished(AiState next)
        {
            m_current = next;
            ExecuteCurrentAction();
        }

        void ExecuteCurrentAction() 
        {
            m_current.Execute(m_controller, WorldStateSnapshot);
        }

        void UpdateWorldState()
        {
            BaseNpc npc = m_controller.Character;
            m_worldStateSnapshot.CurrentStats = npc.CurrentStats;
            m_worldStateSnapshot.Enemies = npc.Enemies;
            m_worldStateSnapshot.Allies = npc.Allies;
            m_worldStateSnapshot.BasicSkills = npc.BasicSkills.Skills;
            m_worldStateSnapshot.DrawSkills = npc.DrawSkills.Skills;
        }
    }
}
