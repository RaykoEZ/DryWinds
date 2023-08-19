using UnityEngine;
using System.Collections.Generic;
using Curry.UI;

namespace Curry.Explore
{

    // Enables/disables player input (movement, card play) when the game is resolving scene effects (e.g. card effects)
    public class PlayerInputController : MonoBehaviour
    {
        // Scripts to disable/enable upon interrupting the scene
        [SerializeField] MoveToggle m_movement = default;
        [SerializeField] SelectionManager m_selectInput = default;
        [SerializeField] HandManager m_cardPlay = default;
        [SerializeField] EnemyManager m_enemies = default;
        [SerializeField] RealtimeCountdown m_countdown = default;
        [SerializeField] List<CanvasGroup> m_toControl = default;
        // Collection of interruptors
        [SerializeField] SceneInterruptCollection m_interruptors = default;
        static bool m_sceneInterrupted = false;
        private void Start()
        {
            m_interruptors.Init();
            m_interruptors.OnInterruptBegin += DisableScene;
            m_interruptors.OnInterruptEnd += EnableScene;
            DisableScene();
        }
        protected virtual void EnableScene()
        {
            if (!m_sceneInterrupted) return;
            m_countdown.BeginCountdown();
            m_movement.EnablePlay();
            m_selectInput?.EnableSelection();
            m_cardPlay?.EnablePlay();
            m_enemies?.ResumeEnemyActions();
            m_sceneInterrupted = false;
            foreach(var item in m_toControl) 
            {
                item.alpha = 1f;
                item.interactable = true;
            }
        }
        protected virtual void DisableScene()
        {
            // Don't disable if already disabled
            if (m_sceneInterrupted) return;
            m_countdown.StopCountdown();
            m_movement.DisablePlay();
            m_selectInput?.DisableSelection();
            m_cardPlay?.DisablePlay();
            m_enemies?.StopEnemyActions();
            m_sceneInterrupted = true;
            foreach (var item in m_toControl)
            {
                item.alpha = 0.2f;
                item.interactable = false;
            }
        }
    }
}