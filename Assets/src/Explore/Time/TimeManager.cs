using System;
using System.Collections;
using UnityEngine;
using Curry.Events;
using Curry.UI;

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

    public delegate void OnOutOfTime(int minsSpent);
    public delegate void OnTimeUpdate(int timeSpent, int timeLeft);

    public class TimeManager : MonoBehaviour
    {
        [Range(1, 1000)]
        [SerializeField] int m_timeToClear = default;
        [SerializeField] CurryGameEventListener m_onSpendTime = default;
        [SerializeField] CurryGameEventListener m_onAddTime = default;
        [SerializeField] CurryGameEventTrigger m_onTimeSpent = default;
        [SerializeField] ResourceBar m_gauge = default;
        [SerializeField] GameClock m_clock = default;
        int m_timeLeft;
        int m_timeSpent;
        public event OnOutOfTime OnOutOfTimeTrigger;
        public event OnTimeUpdate OnTimeSpent;

        public int TimeToClear { get { return m_timeToClear; } }
        public int TimeLeft { get { return m_timeLeft; } }

        // Use this for initialization
        void Awake()
        {
            ResetTime();
            m_onSpendTime?.Init();
            m_onAddTime?.Init();
            m_gauge.SetMaxValue(TimeLeft);
            m_gauge.SetBarValue(TimeToClear, forceInstantChange: true);
        }

        public void ResetTime()
        {
            m_timeLeft = m_timeToClear;
            m_timeSpent = 0;
        }

        public void AddTime(EventInfo time)
        {
            if (time is TimeInfo add)
            {
                m_timeLeft += add.Time;
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
            m_timeLeft += time;
        }

        // spend time and check if we run out of time
        public void TrySpendTime(int timeToSpend, out bool enoughTime) 
        {
            enoughTime = m_timeLeft >= timeToSpend;
            if (timeToSpend <= 0)
            {
                return;
            }
            if (enoughTime) 
            {
                StartCoroutine(OnSpendTime(timeToSpend));
            }
        }
        IEnumerator OnSpendTime(int toSpend)
        {
            m_timeLeft -= toSpend;
            m_timeSpent+= toSpend;
            //Trigger event
            OnTimeSpent?.Invoke(toSpend, TimeLeft);
            m_onTimeSpent?.TriggerEvent(new TimeInfo(toSpend));
            if (Mathf.Approximately(m_timeLeft, 0f))
            {
                OnOutOfTimeTrigger?.Invoke(m_timeSpent);
            }
            m_gauge.SetBarValue(m_timeLeft);
            // Animate clock
            for (int i = 0; i < toSpend; ++i)
            {
                m_clock.IncrementMinute();
                yield return new WaitForSeconds(0.02f);
            }

        }
    }
}