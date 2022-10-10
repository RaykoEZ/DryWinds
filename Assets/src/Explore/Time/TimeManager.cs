using System;
using System.Collections;
using UnityEngine;
using Curry.Events;
using Curry.UI;

namespace Curry.Explore
{
    public class TimeArgs : EventArgs 
    {
        float hours;
        public float Hours { get { return hours; } }
        public TimeArgs(float hr) 
        {
            hours = hr;
        }
    }

    public delegate void OnOutOfTime(int minsSpent);
    public delegate void OnTimeUpdate(int minssLeft);

    public class TimeManager : MonoBehaviour
    {
        [Range(1, 1000)]
        [SerializeField] int m_minsToClear = default;
        [SerializeField] CurryGameEventListener m_onSpendTime = default;
        [SerializeField] CurryGameEventListener m_onAddTime = default;
        [SerializeField] ResourceBar m_gauge = default;
        [SerializeField] GameClock m_clock = default;
        int m_minsLeft;
        int m_minsSpent;
        public OnOutOfTime OnOutOfTimeTrigger;
        public OnTimeUpdate OnTimeSpent;

        public int MinutesToClear { get { return m_minsToClear; } }
        public int MinutesLeft { get { return m_minsLeft; } }

        // Use this for initialization
        void Awake()
        {
            ResetTime();
            m_onSpendTime?.Init();
            m_onAddTime?.Init();
            m_gauge.SetMaxValue(MinutesLeft);
            m_gauge.SetBarValue(MinutesToClear, forceInstantChange: true);
        }

        public void ResetTime()
        {
            m_minsLeft = m_minsToClear;
            m_minsSpent = 0;
        }

        public void AddTime(EventInfo time)
        {
            if (time.Payload["spendTime"] is int mins)
            {
                m_minsLeft += mins;
            }
        }

        // spend time and check if we run out of time
        public void SpendTime(EventInfo time)
        {
            if (time.Payload["spendTime"] is int mins)
            {
                TrySpendTime(mins, out _);
            }
        }

        public void AddTime(int mins) 
        {
            m_minsLeft += mins;
        }

        // spend time and check if we run out of time
        public void TrySpendTime(int minsToSpend, out bool enoughTime) 
        {
            enoughTime = m_minsLeft >= minsToSpend;
            if (minsToSpend <= 0)
            {
                return;
            }
            if (enoughTime) 
            {
                StartCoroutine(OnSpendTime(minsToSpend));
            }
        }
        IEnumerator OnSpendTime(int increase)
        {
            m_minsLeft -= increase;
            m_minsSpent+= increase;
            m_gauge.SetBarValue(m_minsLeft);
            // Animate clock
            for (int i = 0; i < increase; ++i)
            {
                m_clock.IncrementMinute();
                yield return new WaitForSeconds(0.05f);
            }
            OnTimeSpent?.Invoke(MinutesLeft);

            if (Mathf.Approximately(m_minsLeft, 0f))
            {
                OnOutOfTimeTrigger?.Invoke(m_minsSpent);
            }
        }
    }
}