using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnTurnPhaseTransition(Type nextState);
    // Responsible for handling turn phase changes triggered from moment-to-moment gameplay.
    public class GamePhaseManager : MonoBehaviour
    {
        [SerializeField] Phase m_initPhase = default;
        [SerializeField] List<Phase> m_allPhases = default;
        Phase m_current;
        Phase m_previous;
        public event OnTurnPhaseTransition OnPhaseChange = default;

        // Add all game states into the dictionary
        Dictionary<Type, Phase> m_turnStateCollection;
        public Phase CurrentPhase { get { return m_current; } }

        private void Awake()
        {
            m_turnStateCollection = new Dictionary<Type, Phase> { };
            foreach(Phase phase in m_allPhases) 
            {
                m_turnStateCollection.Add(phase.GetType(), phase);
            }
            if (!m_turnStateCollection.ContainsValue(m_initPhase)) 
            {
                m_turnStateCollection.Add(m_initPhase.GetType(), m_initPhase);
            }
        }
        private void Start()
        {
            StartGame();
        }
        public void StartGame()
        {
            SetCurrentState(m_initPhase.GetType());
        }
        void SetCurrentState(Type type)
        {
            m_current = m_turnStateCollection[type];
            m_current.Init();
            m_current.OnGameStateTransition += OnStateTransition;
            m_current?.OnEnter(m_previous);
        }
        void OnStateTransition(Type type)
        {
            TransitionToNextState(type);
            OnPhaseChange?.Invoke(type);
        }
        void TransitionToNextState(Type newGameState)
        {
            m_current.OnGameStateTransition -= OnStateTransition;
            m_previous = m_current;
            SetCurrentState(newGameState);
        }
    }

}