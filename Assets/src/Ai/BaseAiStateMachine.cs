﻿using System.Collections;
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

        protected virtual AiState BestState
        {
            get
            {
                List<AiState> states = ValidStates;
                AiState best = m_defaultState;
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

        protected void OnEnable()
        {
            m_npc.OnEvaluate += Evaluate;
        }
        protected void OnDisable()
        {
            m_npc.OnEvaluate -= Evaluate;
        }

        protected virtual void Start() 
        {
            m_current = m_defaultState;
            ResolveCurrentState();
        }

        // Determine state changes or additional behaviour
        public virtual void Evaluate() 
        {
            UpdateWorldState();
            AiState newState = BestState;
            if (newState != m_current) 
            {
                TransitionTo(newState);
            }
            else 
            {
                ResolveCurrentState();
            }
        }

        protected virtual void TransitionTo(AiState next)
        {
            StartCoroutine(m_current.OnTransition(next, OnTransitionFinished));
        }

        protected virtual void OnTransitionFinished(AiState next)
        {
            m_current = next;
            ResolveCurrentState();
        }

        void ResolveCurrentState() 
        {
            m_current.ResolveState(m_controller, WorldStateSnapshot);
        }

        void UpdateWorldState()
        {
            m_worldStateSnapshot.Self = m_npc;
            m_worldStateSnapshot.CurrentStats = m_npc.CurrentStats;
            m_worldStateSnapshot.Enemies = m_npc.Enemies;
            m_worldStateSnapshot.Allies = m_npc.Allies;
            m_worldStateSnapshot.BasicSkills = m_npc.BasicSkills.Skills;
            m_worldStateSnapshot.DrawSkills = m_npc.DrawSkills.Skills;
        }
    }
}
