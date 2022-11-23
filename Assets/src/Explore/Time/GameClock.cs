using System;
using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    [Serializable]
    public struct TimeOfDay
    {
        [Range(0, 23)]
        public int Hour;
    }
    public class GameClock : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_dayCountLabel = default;
        [SerializeField] TextMeshProUGUI m_timeLabel = default;
        int m_dayCount = 0;
        TimeOfDay m_timeOfDay = new TimeOfDay {Hour = 0};
        public TimeOfDay ClockTime { get { return m_timeOfDay; } }
        public void IncrementMinute()
        {
            if(m_timeOfDay.Hour == 23) 
            {
                m_timeOfDay.Hour = 0;
                IncrementDay();
            }
            else 
            {
                m_timeOfDay.Hour++;
            }
            UpdateClockLabels();
        }

        public void IncrementDay()
        {
            m_dayCount++;
            UpdateDayLabel();
        }

        // Future: add animation?
        void UpdateDayLabel()
        {
            m_dayCountLabel.text = m_dayCount.ToString();
        }

        // Future: add animation?
        void UpdateClockLabels()
        {
            // Add zero to display text if we only have one digit
            string minute = m_timeOfDay.Hour > 9 ? m_timeOfDay.Hour.ToString() : $"0{m_timeOfDay.Hour}";
            m_timeLabel.text = minute;
        }
    }
}