using UnityEngine;
using Curry.Util;
namespace Curry.Explore
{

    // Enables/disables player input (movement, card play) when the game is resolving scene effects (e.g. card effects)
    public class PlayerInputController : MonoBehaviour
    {
        // Scripts to disable/enable upon interrupting the scene
        [SerializeField] MovementManager m_movement = default;
        [SerializeField] SelectionManager m_selectInput = default;
        [SerializeField] HandManager m_cardPlay = default;

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
            m_movement.EnablePlay();
            m_selectInput?.EnableSelection();
            m_cardPlay?.EnablePlay();
            m_sceneInterrupted = false;
        }
        protected virtual void DisableScene()
        {
            // Don't disable if already disabled
            if (m_sceneInterrupted) return;
            m_movement.DisablePlay();
            m_selectInput?.DisableSelection();
            m_cardPlay?.DisablePlay();
            m_sceneInterrupted = true;
        }
    }
}