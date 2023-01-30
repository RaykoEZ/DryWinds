using UnityEngine;
using TMPro;
using Curry.Game;
using Curry.Events;
using Curry.Util;
using System.Collections.Generic;

namespace Curry.Explore
{
    public struct GameConditionContext 
    {
        public int TimeLeft;
        public GameClock.TimeOfDay TimeOfDay;
        public IPlayer Player;
    }

    public class GameStateController : MonoBehaviour
    {
        [SerializeField] Adventurer m_player = default;
        [SerializeField] Animator m_gameResult = default;
        [SerializeField] ObjectiveManager m_objectives = default;
        [SerializeField] TextMeshProUGUI m_resultText = default;
        [SerializeField] GameConditionAttribute m_conditions = default;
        void Start() 
        {
            m_gameResult.gameObject.SetActive(false);
            m_player.OnDefeat += OnPlayerDefeat;
            m_objectives.OnCriticalFailure += OnCriticalFail;
            m_objectives.AllCriticalComplete += OnGameCleared;
        }
        void OnPlayerDefeat(IPlayer player) 
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
            m_resultText.text = "Commission Completed";
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
