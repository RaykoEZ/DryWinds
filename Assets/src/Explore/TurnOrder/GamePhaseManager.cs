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
        Phase m_previous;
        public event OnTurnPhaseTransition OnPhaseChange = default;
        // For Interrupting current phases
        Stack<Phase> m_phaseStack = new Stack<Phase>();
        // Add all game states into the dictionary
        Dictionary<Type, Phase> m_turnStateCollection = new Dictionary<Type, Phase>();
        protected Phase CurrentPhase { get { return m_phaseStack.Peek(); } }

        private void Awake()
        {
            m_turnStateCollection = new Dictionary<Type, Phase> { };
            foreach(Phase phase in m_allPhases) 
            {
                phase.Init();
                m_turnStateCollection.Add(phase.GetType(), phase);
                phase.OnInterrupt += HandleInterrupt;
            }
            if (!m_turnStateCollection.ContainsValue(m_initPhase)) 
            {
                m_initPhase.Init();
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
        void HandleInterrupt(Phase interrupt) 
        {
            if (interrupt == null) return;

            m_previous = CurrentPhase;
            // Pause current phase (e.g. UI)
            m_previous.Pause();
            m_phaseStack.Push(interrupt);
            interrupt.OnGameStateTransition += InterruptResolved;
            interrupt.OnEnter(m_previous);
        }
        void InterruptResolved(Type _) 
        {
            m_previous = m_phaseStack.Pop();
            m_previous.OnGameStateTransition -= InterruptResolved;
            // Resume previous phase operations
            CurrentPhase.Resume();
        }
        void SetCurrentState(Type type)
        {
            m_phaseStack.Push(m_turnStateCollection[type]);
            CurrentPhase.OnGameStateTransition += OnStateTransition;
            CurrentPhase?.OnEnter(m_previous);
        }
        void OnStateTransition(Type type)
        {
            TransitionToNextState(type);
            OnPhaseChange?.Invoke(type);
        }
        void TransitionToNextState(Type newGameState)
        {
            m_previous = m_phaseStack.Pop();
            m_previous.OnGameStateTransition -= OnStateTransition;
            SetCurrentState(newGameState);
        }
    }

}