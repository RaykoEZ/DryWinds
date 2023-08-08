using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Curry.UI.Tutorial
{
    public class TutorialVideo : UIPage
    {
        [SerializeField] VideoPlayer m_video = default;
        public override void Play()
        {
            UIAnim?.Show();
            m_video?.Play();
        }
        public override void Stop()
        {
            UIAnim?.Hide();
            m_video?.Stop();
        }
    }
}