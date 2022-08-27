using System;
using UnityEngine;
using Curry.Events;

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

    public delegate void OnOutOfTime(float hoursSpent);

    public class TimeManager : MonoBehaviour
    {
        [Range(1f, 1000f)]
        [SerializeField] float m_hoursToClear = default;
        [SerializeField] CurryGameEventListener m_onSpendTime = default;
        [SerializeField] CurryGameEventListener m_onAddTime = default;

        float m_hoursLeft;
        float m_hoursSpent;
        public OnOutOfTime OnOutOfTimeTrigger;
        public float HoursToClear { get { return m_hoursToClear; } }
        public float HoursLeft { get { return m_hoursLeft; } }

        // Use this for initialization
        void Awake()
        {
            ResetTime();
        }

        public void ResetTime()
        {
            m_hoursLeft = m_hoursToClear;
            m_hoursSpent = 0f;
        }

        public void AddTime(EventInfo time)
        {
            if (time.Payload["hour"] is float hour)
            {
                m_hoursLeft += hour;
            }
        }

        // spend time and check if we run out of time
        public void SpendTime(EventInfo time)
        {
            if (time.Payload["hour"] is float hour)
            {
                SpendTime(hour, out _);
            }
        }

        public void AddTime(float hours) 
        {
            m_hoursLeft += hours;
        }

        // spend time and check if we run out of time
        public void SpendTime(float hoursToSpend, out bool enoughTime) 
        {
            enoughTime = m_hoursLeft >= hoursToSpend;
            if (enoughTime) 
            {
                m_hoursLeft -= hoursToSpend;
                m_hoursSpent += hoursToSpend;
            }
            
            if(Mathf.Approximately(m_hoursLeft, 0f)) 
            {
                OnOutOfTimeTrigger?.Invoke(m_hoursSpent);
            }
        }
    }
}