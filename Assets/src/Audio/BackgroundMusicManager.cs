using System.Collections;
using UnityEngine;

namespace Curry.UI
{
    // Plays and changes background music clip
    public class BackgroundMusicManager : MonoBehaviour 
    {
        [SerializeField] AudioSource m_bgmSource = default;
        [SerializeField] float m_fadeDuration = default;
        [SerializeField] AnimationCurve m_fadeInCurve = default;
        [SerializeField] AnimationCurve m_fadeOutCurve = default;
        void Start()
        {
            StartCoroutine(FadeIn());
            m_bgmSource?.Play();
        }
        public void ChangeBGM(AudioClip clip, bool playNow = true) 
        {
            if (playNow) 
            {
                StartCoroutine(BgmTransition_Internal(clip));
            }
            else 
            {
                m_bgmSource.clip = clip;
            }
        }
        IEnumerator BgmTransition_Internal(AudioClip clip) 
        {
            // Fade out old bgm
            yield return StartCoroutine(FadeOut());
            m_bgmSource.Stop();
            yield return new WaitForEndOfFrame();
            // start new bgm
            m_bgmSource.clip = clip;
            m_bgmSource.Play();
            yield return StartCoroutine(FadeIn());
        }
        IEnumerator FadeOut() 
        {
            float t = 0f;
            while (t < m_fadeDuration)
            {
                t += Time.deltaTime;
                m_bgmSource.volume = m_fadeOutCurve.Evaluate(t / m_fadeDuration);
                yield return null;
            }
        }
        IEnumerator FadeIn() 
        {
            float t = 0f;
            while (t < m_fadeDuration)
            {
                t += Time.deltaTime;
                m_bgmSource.volume = m_fadeInCurve.Evaluate(t/m_fadeDuration);
                yield return null;
            }
        }
    }
}