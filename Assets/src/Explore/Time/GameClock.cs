using System;
using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    [Serializable]
    public class TimeOfDay
    {
        [Range(0, 23)]
        [SerializeField] int m_currentHour = default;
        [Range(0, 8)]
        [SerializeField] int m_dayBreakAt = default;
        [Range(13, 21)]
        [SerializeField] int m_nightfallAt = default;
        public int Hour { get { return m_currentHour; } set { m_currentHour = value; } }
        public int DayBreakAt { get { return m_dayBreakAt; } }
        public int NightfallAt { get { return m_nightfallAt; } }        
    }
    public delegate void TimeOfDayChange(int Hour);
    public class GameClock : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_dayCountLabel = default;
        [SerializeField] TextMeshProUGUI m_timeLabel = default;
        [SerializeField] TextMeshProUGUI m_dayNightLabel = default;
        [SerializeField] TimeOfDay m_timeOfDay = default;
        public event TimeOfDayChange OnDaybreak;
        public event TimeOfDayChange OnNightfall;
        int m_dayCount = 0;

        void Start()
        {
            HandleDayNightCycle();
        }
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
            HandleDayNightCycle();
            UpdateClockLabels();
        }
        void HandleDayNightCycle()
        {
            int time = m_timeOfDay.Hour;
            if (time >= m_timeOfDay.NightfallAt || time < m_timeOfDay.DayBreakAt)
            {
                m_dayNightLabel.text = "Nighttime";
                OnNightfall?.Invoke(time);
            }
            else if (time >= m_timeOfDay.DayBreakAt && time < m_timeOfDay.NightfallAt)
            {
                m_dayNightLabel.text = "Daytime";
                OnDaybreak?.Invoke(time);
            }
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