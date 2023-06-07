using System;
using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    [Serializable]
    public class TimeSettings
    { 
        [Range(0, 23)]
        [SerializeField] int m_currentHour = default;
        [Range(0, 8)]
        [SerializeField] int m_dayBreakAt = default;
        [Range(13, 21)]
        [SerializeField] int m_nightfallAt = default;
        public int Hour { get { return m_currentHour; } set { m_currentHour = Mathf.Clamp(value, 0, 23); } }
        public int DayBreakAt { get { return m_dayBreakAt; } set { m_dayBreakAt = Mathf.Clamp(value, 0, 23); } }
        public int NightfallAt { get { return m_nightfallAt; } set { m_nightfallAt = Mathf.Clamp(value, 0, 23); } }
    }
    public delegate void OnTimeUpdate(int dayCount, int Hour);
    public class GameClock : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_dayCountLabel = default;
        [SerializeField] TextMeshProUGUI m_timeLabel = default;
        [SerializeField] TimeSettings m_setting = default;
        public event OnTimeUpdate OnTimeElapsed;
        public int DayCount { get; protected set; } = 0;
        public int CurrentTime => m_setting.Hour;

        void Start()
        {
            UpdateClockDisplay();
        }
        public void Increment()
        {
            if(m_setting.Hour == 23) 
            {
                m_setting.Hour = 0;
                DayCount++;
                UpdateDayCountDisplay();
            }
            else 
            {
                m_setting.Hour++;
            }
            OnTimeElapsed?.Invoke(DayCount, CurrentTime);
            UpdateClockDisplay();
        }
        // Future: add animation?
        void UpdateDayCountDisplay()
        {
            m_dayCountLabel.text = DayCount.ToString();
        }

        // Future: add animation?
        void UpdateClockDisplay()
        {
            // Add zero to display text if we only have one digit
            string time = m_setting.Hour > 9 ? m_setting.Hour.ToString() : $"0{m_setting.Hour}";
            m_timeLabel.text = time;
        }
    }
}