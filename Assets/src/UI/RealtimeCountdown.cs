using Curry.Util;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Curry.UI
{
    public delegate void OnCountdownFinish();
    [RequireComponent(typeof(CoroutineManager))]
    // Handles displaying a countdown timer with TextMeshPro in seconds in game time (second)
    public class RealtimeCountdown : MonoBehaviour
    {
        [SerializeField] protected int m_initTimerValueInSeconds = default;
        [SerializeField] protected TextMeshProUGUI m_display = default;
        [SerializeField] protected CoroutineManager m_coroutineManager = default;
        protected int m_currentTimer = 0;
        public event OnCountdownFinish OnFinish;
        void Start()
        {
            m_currentTimer = m_initTimerValueInSeconds;     
        }
        // Use this for initialization
        public void BeginCountdown()
        {
            m_coroutineManager.ScheduleCoroutine(Countdown_Internal(), true);
        }
        IEnumerator Countdown_Internal() 
        {
            while(m_currentTimer > 0) 
            {
                m_display.text = m_currentTimer.ToString();
                yield return new WaitForSeconds(1f);
                m_currentTimer--;
            }
            OnFinish?.Invoke();
        }
        // Update is called once per frame
        public void StopCountdown()
        {
            m_coroutineManager.StopAll();
        }
        public void ResetCountdown(int newInitTime = 0)
        {
            m_coroutineManager.StopAllCoroutines();
            m_currentTimer = newInitTime <= 0 ? m_initTimerValueInSeconds : newInitTime;
        }
    }
}