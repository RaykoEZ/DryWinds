using UnityEngine;
using TMPro;
using Curry.Game;
using Curry.Events;
using Curry.Util;
using System.Collections;

namespace Curry.Explore
{
    // A snapshot of the current game state
    public struct GameStateContext
    {
        public int TimeLeft { get; private set; }
        public IPlayer Player { get; private set; }
        public DeckManager Deck { get; private set; }
        public LootManager LootManager { get; private set; }
        public GameConditionAttribute Milestones { get; private set; }
        public GameStateContext(
            int timeLeft, 
            IPlayer player,
            DeckManager deck,
            LootManager loot,
            GameConditionAttribute milestones) 
        {
            TimeLeft = timeLeft;
            Player = player;
            Deck = deck;
            LootManager = loot;
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
        [SerializeField] LootManager m_loot = default;
        [SerializeField] GamePhaseManager m_gamePhase = default;
        [SerializeField] TextMeshProUGUI m_resultText = default;
        [SerializeField] GameConditionAttribute m_mileStones = default;
        [SerializeField] CurryGameEventListener m_onConditionAchieved = default;
        bool GameReadyToStart => m_deck.IsReady;
        public GameStateContext GetCurrent() 
        {
            int timeLeft = m_time.TimeLeftToClear;
            GameStateContext ret = new GameStateContext(timeLeft, m_player, m_deck, m_loot, m_mileStones);
            return ret;
        }
        void Start() 
        {
            m_onConditionAchieved?.Init();
            m_gameResult.gameObject.SetActive(false);
            m_player.OnDefeat += OnPlayerDefeat;
            m_objectives.OnCriticalFailure += OnCriticalFail;
            m_objectives.AllCriticalComplete += OnGameCleared;
            StartCoroutine(StartGame_Interal());
        }
        IEnumerator StartGame_Interal() 
        {
            yield return new WaitUntil(() => GameReadyToStart);
            m_gamePhase.StartGame();
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
