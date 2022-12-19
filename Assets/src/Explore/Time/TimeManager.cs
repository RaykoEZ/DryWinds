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

    public delegate void OutOfTime(int timeSpent);
    public delegate void TimeSpent(int timeSpent, int timeLeft);
    public delegate void ClockTimeUpdate(TimeOfDay time);
    public class TimeManager : MonoBehaviour
    {
        [Range(1, 1000)]
        [SerializeField] int m_timeToClear = default;
        [SerializeField] CurryGameEventListener m_onPlayerTurn = default;
        [SerializeField] CurryGameEventListener m_onSpendTime = default;
        [SerializeField] CurryGameEventListener m_onAddTime = default;
        [SerializeField] CurryGameEventTrigger m_onTimeSpent = default;
        [SerializeField] ResourceBar m_gauge = default;
        [SerializeField] GameClock m_clock = default;
        [SerializeField] TextMeshProUGUI m_turnTimer = default;
        int m_timeLeftToClear;
        int m_timeSpent;
        public event OutOfTime OnOutOfTimeTrigger;
        public event TimeSpent OnTimeSpent;
        public int TimeLeftThisTurn { get; protected set; }
        public int TimeToClear { get { return m_timeToClear; } }
        public int TimeLeftToClear { get { return m_timeLeftToClear; } }
        // Use this for initialization
        void Awake()
        {
            ResetTime();
            m_onPlayerTurn?.Init();
            m_onSpendTime?.Init();
            m_onAddTime?.Init();
            m_gauge.SetMaxValue(TimeLeftToClear);
            m_gauge.SetBarValue(TimeToClear, forceInstantChange: true);
        }
        public void OnPlayerTurn(EventInfo info)
        {
            if (info is TimeInfo time)
            {
                TimeLeftThisTurn = time.Time;
                UpdateTurnTimer();
            }
        }
        public void ResetTime()
        {
            m_timeLeftToClear = m_timeToClear;
            m_timeSpent = 0;
        }

        public void AddTime(EventInfo time)
        {
            if (time is TimeInfo add)
            {
                m_timeLeftToClear += add.Time;
            }
        }

        // spend time and check if we run out of time
        public void SpendTime(EventInfo time)
        {
            if (time is TimeInfo spend)
            {
                TrySpendTime(spend.Time, out _);
            }
        }

        public void AddTime(int time) 
        {
            m_timeLeftToClear += time;
        }

        // spend time and check if we run out of time
        public void TrySpendTime(int timeToSpend, out bool enoughTime) 
        {
            enoughTime = m_timeLeftToClear >= timeToSpend &&
                TimeLeftThisTurn >= timeToSpend;
            if (timeToSpend <= 0)
            {
                return;
            }
            if (enoughTime) 
            {
                StartCoroutine(OnSpendTime(timeToSpend));
            }
        }

        void UpdateTurnTimer()
        {
            m_turnTimer.text = TimeLeftThisTurn.ToString();
        }
        IEnumerator OnSpendTime(int toSpend)
        {
            TimeLeftThisTurn -= toSpend;
            m_timeLeftToClear -= toSpend;
            UpdateTurnTimer();
            m_timeSpent += toSpend;
            //Trigger event
            OnTimeSpent?.Invoke(toSpend, TimeLeftToClear);
            m_onTimeSpent?.TriggerEvent(new TimeInfo(toSpend));
            if (Mathf.Approximately(m_timeLeftToClear, 0f))
            {
                OnOutOfTimeTrigger?.Invoke(m_timeSpent);
            }
            m_gauge.SetBarValue(m_timeLeftToClear);
            // Animate clock
            for (int i = 0; i < toSpend; ++i)
            {
                m_clock.IncrementMinute();
                yield return new WaitForSeconds(0.02f);
            }

        }
    }
}