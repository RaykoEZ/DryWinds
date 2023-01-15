using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnTurnPhaseTransition(Type nextState);
    // Responsible for handling turn phase changes triggered from moment-to-moment gameplay.
    public class GamePhaseManager : MonoBehaviour
    {
        [SerializeField] GamePhaseChangePopup m_phasePopup = default;
        [SerializeField] Phase m_initPhase = default;
        [SerializeField] List<Phase> m_allPhases = default;
        Phase m_previous;
        public event OnTurnPhaseTransition OnPhaseChange = default;
        // For Interrupting current phases
        Stack<Phase> m_phaseStack = new Stack<Phase>();
        // Add all game states into the dictionary
        Dictionary<Type, Phase> m_turnStateCollection = new Dictionary<Type, Phase>();
        protected Phase CurrentPhase { get { return m_phaseStack.Peek(); } }

        void Awake()
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
        void Start()
        {
            StartGame();
        }
        void StartGame()
        {
            SetCurrentState(m_initPhase.GetType());
        }
        void HandleInterrupt(Phase interrupt) 
        {
            if (interrupt == null) return;
            // interrupt current state, unlisten transition callbacks
            CurrentPhase.OnGameStateTransition -= OnStateTransition;
            m_previous = CurrentPhase;
            // Pause current phase (e.g. UI)
            m_previous.Pause();

            Action onInterrupt = () => {
                m_phaseStack.Push(interrupt);
                OnPhaseChange?.Invoke(interrupt.GetType());
                interrupt.OnGameStateTransition += InterruptResolved;
                interrupt.OnEnter(m_previous);
            };
            // Make popup for interrupt state
            m_phasePopup.ShowPopup(interrupt.Name, onInterrupt);
        }
        void InterruptResolved(Type _) 
        {
            // unlisten from finished interrupt state
            m_previous = m_phaseStack.Pop();
            m_previous.OnGameStateTransition -= InterruptResolved;
            OnPhaseChange?.Invoke(CurrentPhase.GetType());
            // Back from interrupt, listen to transition callbacks
            CurrentPhase.OnGameStateTransition += OnStateTransition;
            // Resume previous phase operations
            CurrentPhase.Resume();
        }
        void SetCurrentState(Type type)
        {
            Phase nextPhase = m_turnStateCollection[type];

            Action change = () => {
                m_phaseStack.Push(nextPhase);
                CurrentPhase.OnGameStateTransition += OnStateTransition;
                CurrentPhase?.OnEnter(m_previous);
            };
            StartCoroutine(ChangeState(nextPhase.Name, change));
        }
        IEnumerator ChangeState(string displayName, Action onChange) 
        {
            // wait for phase to finish evaluating
            yield return new WaitWhile(() => m_phasePopup.AnimationInProgress);
            m_phasePopup.ShowPopup(displayName, onChange);
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