using Curry.Explore;
using Curry.Util;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Curry.UI
{
    public delegate void OnCountdownUpdate();
    public interface ICountdown 
    {
        event OnCountdownUpdate OnFinish;
        event OnCountdownUpdate OnTick;
        void BeginCountdown();
        void MultiplyCountdownSpeed(float multiplier);
        void StopCountdown();
        void ResetCountdown(int newInitTime = 0);
    }
    [RequireComponent(typeof(CoroutineManager))]
    // Handles displaying a countdown timer with TextMeshPro in seconds in game time (second)
    public class RealtimeCountdown : TimeManager, ICountdown
    {
        [Range(1, 999)]
        [SerializeField] protected int m_defaultTimeInSeconds = default;
        [SerializeField] protected TextMeshProUGUI m_display = default;
        [SerializeField] protected Gradient m_colorOverTime = default;
        [SerializeField] protected CoroutineManager m_coroutineManager = default;
        [SerializeField] protected AudioManager m_audio = default;
        protected const float c_baseWaitTime = 1f;
        protected float m_speedMultipier = 1f;
        protected int m_currentTimer = 0;
        public event OnCountdownUpdate OnFinish;
        event OnCountdownUpdate OnFinish_Internal;
        public event OnCountdownUpdate OnTick;

        protected float CurrentCountdownTimeInterval => 
            Mathf.Clamp(c_baseWaitTime/m_speedMultipier, 0.1f, 5f);
        public override int TimeToClear => m_defaultTimeInSeconds;
        public override int TimeLeftToClear => m_currentTimer;
        void Awake() 
        {
            m_currentTimer = m_defaultTimeInSeconds;
        }
        void Start()
        {
            OnFinish_Internal += ReplayCountdown;
        }
        // Use this for initialization
        public void BeginCountdown()
        {
            m_coroutineManager.ScheduleCoroutine(Countdown_Internal(), true);
        }
        public void MultiplyCountdownSpeed(float multiplier) 
        {
            m_speedMultipier *= multiplier >= 0f ? multiplier : 1f;
        }
        IEnumerator Countdown_Internal() 
        {
            float t;
            bool alertTime = false;
            while (m_currentTimer > 0) 
            {
                t = (float)m_currentTimer / (float)m_defaultTimeInSeconds;
                m_display.color = m_colorOverTime.Evaluate(t);
                // alert on < 10% time
                if (t < 0.1f && !alertTime) 
                {
                    alertTime = true;
                    m_audio?.Play("timeRedAlert");
                }
                m_display.text = m_currentTimer.ToString();
                yield return new WaitForSeconds(CurrentCountdownTimeInterval);
                m_currentTimer--;
                OnTick?.Invoke();
                NotifyTimeSpent(1);
            }
            alertTime = false;
            OnFinish_Internal?.Invoke();
            OnFinish?.Invoke();
            OnOutOfTime();
        }
        // Update is called once per frame
        public void StopCountdown()
        {
            m_coroutineManager.StopAll();
        }
        public void ResetCountdown(int newInitTime = 0)
        {
            m_coroutineManager.StopAllCoroutines();
            m_speedMultipier = 1f;
            m_currentTimer = newInitTime <= 0 ? m_defaultTimeInSeconds : newInitTime;
        }
        public override void AddTime(int time)
        {
            m_currentTimer += time;
        }
        public override bool TrySpendTime(int timeToSpend)
        {
            bool enoughTime = TimeLeftToClear >= timeToSpend;
            if (enoughTime)
            {
                m_currentTimer -= timeToSpend;
                NotifyTimeSpent(timeToSpend);
            }
            return enoughTime;
        }
        public override void SpendTime(int timeToSpend)
        {
            int temp = m_currentTimer;
            m_currentTimer = Mathf.Max(m_currentTimer - timeToSpend, 0);
            NotifyTimeSpent(temp - m_currentTimer);

        }
        public override void SetTime(int time)
        {
            m_currentTimer = time;
        }

        public override void SetMaxTime(int maxTime)
        {
            m_defaultTimeInSeconds = maxTime;
        }
        protected void ReplayCountdown() 
        {
            ResetCountdown();
            BeginCountdown();
        }
    }
}