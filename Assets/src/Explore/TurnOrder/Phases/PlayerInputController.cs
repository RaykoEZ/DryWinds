using UnityEngine;
using System.Collections.Generic;
using Curry.UI;

namespace Curry.Explore
{

    // Enables/disables player input (movement, card play) when the game is resolving scene effects (e.g. card effects)
    public class PlayerInputController : MonoBehaviour
    {
        // Scripts to disable/enable upon interrupting the scene
        [SerializeField] ActionCounter m_apCounter = default;
        [SerializeField] MoveToggle m_movement = default;
        [SerializeField] SelectionManager m_selectInput = default;
        [SerializeField] HandManager m_cardPlay = default;
        [SerializeField] EnemyManager m_enemies = default;
        [SerializeField] RealtimeCountdown m_countdown = default;
        [SerializeField] PauseGame m_pauseToggle = default;
        [SerializeField] List<CanvasGroup> m_toControl = default;
        // Collection of interruptors
        [SerializeField] SceneInterruptCollection m_interruptors = default;
        static bool s_sceneInterrupted = false;
        static bool s_playerPausedScene = false;
        public static bool SceneInterrupted => s_sceneInterrupted;
        public static bool PlayerPausedScene => s_playerPausedScene;

        private void Start()
        {
            m_interruptors.Init();
            m_interruptors.OnInterruptBegin += DisableScene;
            m_interruptors.OnInterruptEnd += EnableScene;
            m_pauseToggle.OnGamePause += PauseGame;
            m_pauseToggle.OnGameResume += ResumeGame;
        }
        protected void PauseGame() 
        {
            s_playerPausedScene = true;
            DisableScene();
        }
        protected void ResumeGame() 
        {
            s_playerPausedScene = false;
            EnableScene();
        }
        protected virtual void EnableScene()
        {
            if (!SceneInterrupted || s_playerPausedScene) return;
            m_countdown.BeginCountdown();
            m_movement.EnablePlay();
            m_selectInput?.EnableSelection();
            m_cardPlay?.EnablePlay();
            m_enemies?.ResumeEnemyActions();
            m_apCounter?.StartChargingAp();
            s_sceneInterrupted = false;
            foreach(var item in m_toControl) 
            {
                item.alpha = 1f;
                item.interactable = true;
            }
        }
        protected virtual void DisableScene()
        {
            // Don't disable if already disabled
            if (SceneInterrupted) return;
            m_countdown.StopCountdown();
            m_movement.DisablePlay();
            m_selectInput?.DisableSelection();
            m_apCounter?.PauseApCharging();
            m_cardPlay?.DisablePlay();
            m_enemies?.StopEnemyActions();
            s_sceneInterrupted = true;
            foreach (var item in m_toControl)
            {
                item.alpha = 0.2f;
                item.interactable = false;
            }
        }
    }
}