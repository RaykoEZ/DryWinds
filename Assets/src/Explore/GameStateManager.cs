using UnityEngine;
using TMPro;
using Curry.Game;
using Curry.Events;
using Curry.Util;
using System.Collections.Generic;

namespace Curry.Explore
{
    // A snapshot of the current game state
    public struct GameStateContext
    {
        public int TimeLeft { get; private set; }
        public GameClock.TimeOfDay TimeOfDay { get; private set; }
        public IPlayer Player { get; private set; }
        public DeckManager Deck { get; private set; }
        public GameConditionAttribute Milestones { get; private set; }
        public GameStateContext(
            int timeLeft, 
            GameClock.TimeOfDay timeOfDay, 
            IPlayer player,
            DeckManager deck,
            GameConditionAttribute milestones) 
        {
            TimeLeft = timeLeft;
            TimeOfDay = timeOfDay;
            Player = player;
            Deck = deck;
            Milestones = milestones;
        }
    }
    public class GameConditionEvent : EventInfo 
    { 
        public GameConditionAttribute ConditionsFulfilled { get; protected set; }

        public GameConditionEvent(GameConditionAttribute conditions) 
        {
            ConditionsFulfilled = conditions;
        }
    }

    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] Adventurer m_player = default;
        [SerializeField] Animator m_gameResult = default;
        [SerializeField] ObjectiveManager m_objectives = default;
        [SerializeField] TimeManager m_time = default;
        [SerializeField] DeckManager m_deck = default;
        [SerializeField] TextMeshProUGUI m_resultText = default;
        [SerializeField] GameConditionAttribute m_mileStones = default;
        [SerializeField] CurryGameEventListener m_onConditionAchieved = default;

        public GameStateContext GetCurrent() 
        {
            int timeLeft = m_time.TimeLeftToClear;
            GameClock.TimeOfDay timeOfDay = m_time.Clock.CurrentTimeOfDay;
            GameStateContext ret = new GameStateContext(timeLeft, timeOfDay, m_player, m_deck, m_mileStones);
            return ret;
        }
        void Start() 
        {
            m_onConditionAchieved?.Init();
            m_gameResult.gameObject.SetActive(false);
            m_player.OnDefeat += OnPlayerDefeat;
            m_objectives.OnCriticalFailure += OnCriticalFail;
            m_objectives.AllCriticalComplete += OnGameCleared;
        }
        public void OnGameConditionFulfilled(EventInfo info) 
        {
            if (info == null) 
            { 
                return; 
            }
            else if (info is GameConditionEvent conditions && 
                conditions.ConditionsFulfilled.ConditionSet == m_mileStones.ConditionSet)
            {
                m_mileStones.Flag |= conditions.ConditionsFulfilled.Flag;
            }
        }

        void OnPlayerDefeat(ICharacter player) 
        {
            m_resultText.text = "Game Over";
            UpdateResultPanel();
        }
        void OnCriticalFail(IObjective objective) 
        {
            m_resultText.text = "Game Over";
            UpdateResultPanel();
        }
        void OnGameCleared()
        {
            m_resultText.text = "Main Objectives Complete";
            UpdateResultPanel();
        }

        void UpdateResultPanel() 
        {
            m_gameResult.gameObject.SetActive(true);
            m_gameResult.SetBool("GameOver", true);
        }

        void OnProceedToResult() 
        {
            m_gameResult.SetBool("GameOver", false);
            m_gameResult.gameObject.SetActive(false);
        }
    }
}
