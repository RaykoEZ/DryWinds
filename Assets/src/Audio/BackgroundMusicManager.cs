using System.Collections;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Curry.UI
{
    // Plays and changes background music clip
    public class BackgroundMusicManager : MonoBehaviour 
    {
        [SerializeField] AudioSource m_bgmSource = default;
        [SerializeField] float m_fadeInDuration = default;
        [SerializeField] float m_fadeOutDuration = default;
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
                m_bgmSource.clip = clip;
            }
            else 
            {
                StartCoroutine(BgmTransition_Internal(clip));
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
        public IEnumerator FadeOut() 
        {
            float t = 0f;
            while (t < m_fadeInDuration)
            {
                t += Time.deltaTime;
                m_bgmSource.volume = m_fadeOutCurve.Evaluate(t / m_fadeOutDuration);
                yield return null;
            }
        }
        IEnumerator FadeIn() 
        {
            float t = 0f;
            while (t < m_fadeInDuration)
            {
                t += Time.deltaTime;
                m_bgmSource.volume = m_fadeInCurve.Evaluate(t/m_fadeInDuration);
                yield return null;
            }
        }
    }
}