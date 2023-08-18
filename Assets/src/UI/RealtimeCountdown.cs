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
        [SerializeField] protected int m_defaultTimeInSeconds = default;
        [SerializeField] protected TextMeshProUGUI m_display = default;
        [SerializeField] protected CoroutineManager m_coroutineManager = default;
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
        void Start()
        {
            m_currentTimer = m_defaultTimeInSeconds;
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
            while(m_currentTimer > 0) 
            {
                m_display.text = m_currentTimer.ToString();
                yield return new WaitForSeconds(CurrentCountdownTimeInterval);
                m_currentTimer--;
                OnTick?.Invoke();
                NotifyTimeSpent(1);
            }
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