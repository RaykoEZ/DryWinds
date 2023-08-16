using UnityEngine;
using TMPro;
using Curry.Game;
using Curry.Events;
using Curry.Util;
using System.Collections;
using Curry.UI;
using System.Collections.Generic;
using System.Linq;
namespace Curry.Explore
{
    // A snapshot of the current game state
    public class GameStateContext
    {
        public int TimeLeft { get; private set; }
        public IPlayer Player { get; private set; }
        public TimeManager Time { get; private set; }
        public HandManager Hand { get; private set; }
        public DeckManager Deck { get; private set; }
        public MovementManager Movement { get; private set; }
        public LootManager LootManager { get; private set; }
        public ActionCounter ActionCount { get; private set; }       
        public GameConditionAttribute Milestones { get; private set; }
        public GameStateContext(
            int timeLeft, 
            IPlayer player,
            TimeManager time,
            HandManager hand,
            DeckManager deck,
            MovementManager move,
            LootManager loot,
            ActionCounter action,
            GameConditionAttribute milestones) 
        {
            TimeLeft = timeLeft;
            Player = player;
            Time = time;
            Hand = hand;
            Movement = move;
            Deck = deck;
            LootManager = loot;
            ActionCount = action;
            Milestones = milestones;
        }
    }
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] Adventurer m_player = default;
        [SerializeField] Animator m_gameResult = default;
        [SerializeField] ObjectiveManager m_objectives = default;
        [SerializeField] PlayZone m_cardPlayZone = default;
        [SerializeField] GameIntroduction m_intro = default;
        [SerializeField] CardActivationHandler m_cardTargeting = default;
        [SerializeField] TimeManager m_time = default;
        [SerializeField] TimeDealerManager m_timeDealer = default;
        [SerializeField] HandManager m_hand = default;
        [SerializeField] DeckManager m_deck = default;
        [SerializeField] LootManager m_loot = default;
        [SerializeField] MovementManager m_movement = default;
        [SerializeField] ActionCounter m_actionCount = default;
        [SerializeField] GamePhaseManager m_gamePhase = default;
        [SerializeField] TextMeshProUGUI m_resultText = default;
        [SerializeField] GameConditionAttribute m_mileStones = default;
        [SerializeField] CurryGameEventListener m_onConditionAchieved = default;
        bool GameReadyToContinue => m_deck.IsReady && m_intro.IsReady;
        public GameStateContext GetCurrent() 
        {
            int timeLeft = m_time.TimeLeftToClear;
            GameStateContext ret = new GameStateContext(
                timeLeft,
                m_player,
                m_time,
                m_hand,
                m_deck,
                m_movement,
                m_loot,
                m_actionCount,
                m_mileStones);
            return ret;
        }
        void Start() 
        {
            m_onConditionAchieved?.Init();
            m_gameResult.gameObject.SetActive(false);
            m_player.OnDefeat += OnPlayerDefeat;
            m_time.OnOutOfTimeTrigger += OnOutOfTime;
            m_objectives.OnCriticalFailure += OnCriticalFail;
            m_objectives.OnCriticalComplete += OnGameCleared;
            StartCoroutine(StartGame_Interal());
        }
        IEnumerator StartGame_Interal() 
        {
            m_intro?.GameStartIntro();
            yield return new WaitUntil(() => GameReadyToContinue);
            //initializing context users after deck loaded
            GameStateContext c = GetCurrent();
            m_cardPlayZone?.Init(c);
            m_cardTargeting?.Init(c);
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
        void OnOutOfTime() 
        {
            (m_time as ICountdown)?.MultiplyCountdownSpeed(1.2f);
            m_timeDealer.Begin(GetCurrent());
        }
        void OnPlayerDefeat(ICharacter player) 
        {
            m_resultText.text = "Game Over";
            StartCoroutine(UpdateResultPanel());
        }
        void OnCriticalFail(IObjective objective) 
        {
            m_resultText.text = "Game Over";
            StartCoroutine(UpdateResultPanel());
        }
        void OnGameCleared()
        {
            m_resultText.text = "Main Objectives Complete. Thank you for playing!";
            StartCoroutine(UpdateResultPanel());
        }
        IEnumerator UpdateResultPanel() 
        {
            m_intro?.GameEnd();
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => GameReadyToContinue);
            m_gameResult.gameObject.SetActive(true);
            m_gameResult.SetBool("GameOver", true);
        }
    }
}
