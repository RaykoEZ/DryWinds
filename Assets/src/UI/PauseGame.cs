using Curry.Explore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.UI
{
    public class PauseGame : SceneInterruptBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_title = default;
        [SerializeField] Toggle m_toggle = default;
        public event OnInputInterrupt OnGamePause;
        public event OnInputInterrupt OnGameResume;
        public void SetPause(bool isPaused)
        {
            m_title.text = isPaused ? "Resume" : "Pause";
            if (isPaused) 
            {
                m_anim?.SetBool("pause", true);
                OnGamePause?.Invoke();
            }
            else 
            {
                m_anim?.SetBool("pause", false);
                OnGameResume?.Invoke();
            }
        }
        public void TogglePause() 
        {
            m_toggle.isOn = !m_toggle.isOn;
        }
    }
}