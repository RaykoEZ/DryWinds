using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public class BaseAiStateMachine : MonoBehaviour 
    {
        [SerializeField] protected BaseNpc m_npc = default;
        [SerializeField] protected NpcController m_controller = default;
        [SerializeField] protected AiState m_defaultState = default;
        [SerializeField] protected List<AiState> m_otherStates = default;
        protected float m_timer = 0f;
        protected AiState m_current;
        AiWorldState m_worldStateSnapshot = new AiWorldState();
        protected virtual AiWorldState WorldStateSnapshot
        {
            get
            {
                return m_worldStateSnapshot;
            }
        }

        protected virtual List<AiState> ValidStates
        {
            get
            {
                List<AiState> validActions = new List<AiState>();
                foreach (AiState state in m_otherStates)
                {
                    if (state.PreCondition(WorldStateSnapshot))
                    {
                        validActions.Add(state);
                    }
                }
                return validActions;
            }
        }

        protected virtual AiState BestState()
        {            
            List<AiState> newStates = ValidStates;
            AiState best;
            if (newStates.Count > 0) 
            {
                best = newStates[0];
                for (int i = 0; i < newStates.Count; ++i)
                {
                    if (newStates[i].Priority(WorldStateSnapshot) > best.Priority(WorldStateSnapshot))
                    {
                        best = newStates[i];
                    }
                }
            }
            else 
            {
                best = m_defaultState;
            }
            return best;
            
        }

        protected void OnEnable()
        {
            m_npc.OnEvaluate += OnInteraction;
        }
        protected void OnDisable()
        {
            m_npc.OnEvaluate -= OnInteraction;
        }

        protected virtual void Start() 
        {
            UpdateWorldState();
            TransitionTo(m_defaultState);
        }

        protected virtual void Update() 
        {
            m_timer += Time.deltaTime;
            if (m_timer > 1f) 
            {
                m_timer = 0f;
                Evaluate();
            }
        }

        // Determine state changes or additional behaviour
        protected virtual void Evaluate() 
        {
            UpdateWorldState();
            AiState newState = BestState();
            if (newState != m_current) 
            {
                TransitionTo(newState);
            }
        }

        // Determine what we do with current interaction event
        protected virtual void OnInteraction(InteractionContext c) 
        {
            Evaluate();
        }

        protected virtual void TransitionTo(AiState newState)
        {
            m_current = newState;
            m_current.OnEnter(m_controller, WorldStateSnapshot);
        }

        void UpdateWorldState()
        {
            m_worldStateSnapshot.CurrentStats = m_npc.CurrentStats;
            m_worldStateSnapshot.Enemies = m_npc.Enemies;
            m_worldStateSnapshot.Allies = m_npc.Allies;
            m_worldStateSnapshot.BasicSkills = m_npc.BasicSkills.Skills;
            m_worldStateSnapshot.DrawSkills = m_npc.DrawSkills.Skills;
        }
    }
}
