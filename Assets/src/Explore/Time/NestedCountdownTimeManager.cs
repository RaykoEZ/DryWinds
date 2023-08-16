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
    public class NestedCountdownTimeManager : TimeManager, ICountdown
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
        public override int TimeToClear => m_timeToClear;
        public override int TimeLeftToClear => m_timeLeftToClear;
        public ICountdown Countdown => m_countdown;

        public event OnCountdownFinish OnFinish
        {
            add
            {
                m_countdown.OnFinish += value;
            }

            remove
            {
                m_countdown.OnFinish -= value;
            }
        }

        // Use this for initialization
        void Start()
        {
            ResetTime();
            Countdown.OnFinish += DecrementTime;
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
        public override void SetTime(int time)
        {
            m_gauge.SetCurrentValue(time);
        }
        public override void SetMaxTime(int max)
        {
            m_gauge.SetMaxValue(max);
        }
        // spend time and check if we run out of time
        public void SpendTime(EventInfo time)
        {
            if (time is TimeInfo spend)
            {
                TrySpendTime(spend.Time);
            }
        }
        public override void AddTime(int time) 
        {
            m_timeLeftToClear += time;
            UpdateTurnTimer();
            m_gauge.SetCurrentValue(m_timeLeftToClear);
        }
        // spend time and check if we run out of time
        public override bool TrySpendTime(int timeToSpend) 
        {
            bool enoughTime = m_timeLeftToClear >= timeToSpend;
            if (enoughTime) 
            {
                StartCoroutine(OnSpendTime(timeToSpend));
            }
            return enoughTime;
        }
        public override void SpendTime(int timeToSpend)
        {
            StartCoroutine(OnSpendTime(timeToSpend));
        }
        void DecrementTime() 
        {
            if (TrySpendTime(1)) 
            {
                Countdown.ResetCountdown();
                Countdown.BeginCountdown();
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
            NotifyTimeSpent(toSpend);
            m_onTimeSpent?.TriggerEvent(new TimeInfo(toSpend));
            if (Mathf.Approximately(m_timeLeftToClear, 0f))
            {
                OnOutOfTime();
            }
            m_gauge.SetCurrentValue(m_timeLeftToClear);
            yield return null;
        }
        public void BeginCountdown()
        {
            m_countdown.BeginCountdown();
        }
        public void MultiplyCountdownSpeed(float multiplier)
        {
            m_countdown.MultiplyCountdownSpeed(multiplier);
        }

        public void StopCountdown()
        {
            m_countdown.StopCountdown();
        }

        public void ResetCountdown(int newInitTime = 0)
        {
            m_countdown.ResetCountdown(newInitTime);
        }
    }
}