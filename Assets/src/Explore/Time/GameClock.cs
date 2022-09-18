using System;
using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    [Serializable]
    public struct TimeOfDay
    {
        [Range(0, 60)]
        public int Minute;
    }
    public class GameClock : MonoBehaviour 
    {       
        [SerializeField] TextMeshProUGUI m_dayCountLabel = default;
        [SerializeField] TextMeshProUGUI m_minuteLabel = default;
        int m_dayCount = 0;
        TimeOfDay m_timeOfDay = new TimeOfDay {Minute = 0};

        public void IncrementMinute()
        {
            if(m_timeOfDay.Minute == 60) 
            {
                m_timeOfDay.Minute = 0;
                IncrementDay();
            }
            else 
            {
                m_timeOfDay.Minute++;
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
            string minute = m_timeOfDay.Minute > 9 ? m_timeOfDay.Minute.ToString() : $"0{m_timeOfDay.Minute}";
            m_minuteLabel.text = minute;
        }
    }
}