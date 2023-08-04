using System;
using System.Collections;
using UnityEngine;
using Curry.Events;
using Curry.UI;
using TMPro;
namespace Curry.Explore
{
    public class TimeInfo : EventInfo 
    {
        int time;
        public int Time { get { return time; } }
        public TimeInfo(int t) 
        {
            time = t;
        }
    }
    public delegate void OutOfTime();
    public delegate void TimeSpent(int timeSpent, int timeLeft);
    public class TimeManager : MonoBehaviour
    {
        [Range(1, 1000)]
        [SerializeField] int m_timeToClear = default;
        [SerializeField] CurryGameEventListener m_onSpendTime = default;
        [SerializeField] CurryGameEventTrigger m_onTimeSpent = default;
        [SerializeField] ResourceDisplayHandler m_gauge = default;
        [SerializeField] RealtimeCountdown m_countdown = default;
        [SerializeField] TextMeshProUGUI m_clearTimer = default;
        [SerializeField] TextMeshProUGUI m_costDiff = default;
        int m_timeLeftToClear;
        int m_timeSpent;
        public event OutOfTime OnOutOfTimeTrigger;
        public event TimeSpent OnTimeSpent;
        public int TimeToClear { get { return m_timeToClear; } }
        public int TimeLeftToClear { get { return m_timeLeftToClear; } }
        // Use this for initialization
        void Start()
        {
            ResetTime();
            m_countdown.OnFinish += DecrementTime;
            m_onSpendTime?.Init();
            m_gauge?.SetMaxValue(TimeLeftToClear);
            m_gauge?.SetCurrentValue(TimeToClear, true);
            UpdateTurnTimer();
        }
        public void ResetTime()
        {
            m_timeLeftToClear = m_timeToClear;
            m_timeSpent = 0;
        }
        public void SetTimer(int time, int maxTime)
        {
            m_gauge.SetMaxValue(maxTime);
            m_gauge.SetCurrentValue(time);
        }
        public void MultiplyCountdownSpeed(float multiplier) 
        {
            m_countdown.MultiplyCoundownSpeed(multiplier);
        }
        // spend time and check if we run out of time
        public void SpendTime(EventInfo time)
        {
            if (time is TimeInfo spend)
            {
                TrySpendTime(spend.Time);
            }
        }
        public void AddTime(int time) 
        {
            m_timeLeftToClear += time;
        }
        // spend time and check if we run out of time
        public bool TrySpendTime(int timeToSpend) 
        {
            bool enoughTime = m_timeLeftToClear >= timeToSpend;
            if (enoughTime) 
            {
                StartCoroutine(OnSpendTime(timeToSpend));
            }
            return enoughTime;
        }
        void DecrementTime() 
        {
            if (TrySpendTime(1)) 
            {
                m_countdown.ResetCountdown();
                m_countdown.BeginCountdown();
            }
        }
        public void PreviewCost(int cost) 
        {
            int newVal = Mathf.Clamp(m_timeLeftToClear - cost, 0, m_timeToClear);
            m_gauge?.Preview(newVal);
            m_costDiff.text = $" - {cost}";
            m_costDiff.color = Color.red;
        }
        public void CancelPreview() 
        {
            m_gauge?.TryCancelPreview();
            m_costDiff.color = Color.clear;
        }
        void UpdateTurnTimer()
        {
            if (m_clearTimer == null) return;
            m_clearTimer.text = TimeLeftToClear.ToString();
        }
        IEnumerator OnSpendTime(int toSpend)
        {
            m_timeLeftToClear -= toSpend;
            UpdateTurnTimer();
            m_timeSpent += toSpend;
            //Trigger event
            OnTimeSpent?.Invoke(toSpend, TimeLeftToClear);
            m_onTimeSpent?.TriggerEvent(new TimeInfo(toSpend));
            if (Mathf.Approximately(m_timeLeftToClear, 0f))
            {
                OnOutOfTimeTrigger?.Invoke();
            }
            m_gauge.SetCurrentValue(m_timeLeftToClear);
            yield return null;
        }
    }
}