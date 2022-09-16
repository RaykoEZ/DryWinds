using System.Collections;
using UnityEngine;
using Curry.UI;
using TMPro;
namespace Curry.Explore
{
    public class TimeGauge : MonoBehaviour
    {
        [SerializeField] ResourceBar m_gauge = default;

        public void UpdateTimeLeft(float hoursLeft) 
        {
            m_gauge.SetBarValue(hoursLeft);
        }
        public void UpdateMaxTime(float maxHours)
        {
            m_gauge.SetMaxValue(maxHours);
        }
    }

    public class GameClock : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_dayCountLabel = default;
        [SerializeField] TextMeshProUGUI m_hourLabel = default;
        [SerializeField] TextMeshProUGUI m_minuteLabel = default;
        int m_dayCount = 0;
        [Range(0,24)]
        int m_hour = 0;
        [Range(0, 60)]
        int m_minute = 0;
        public void IncrementMinute()
        {
            if(m_minute == 60) 
            {
                m_minute = 0;
                IncrementHour();
            }
            else 
            {
                m_minute++;
                UpdateClockLabels();
            }
        }
        public void IncrementHour()
        {
            if (m_hour == 24)
            {
                m_hour = 0;
                IncrementDay();
            }
            else
            {
                m_hour++;
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
            string hour = m_hour > 9? m_hour.ToString() : $"0{m_hour}";
            string minute = m_minute > 9 ? m_minute.ToString() : $"0{m_minute}";

            m_hourLabel.text = hour;
            m_minuteLabel.text = minute;
        }
    }

    public class WeatherManager : MonoBehaviour 
    { 
    
    }

    public class WeatherDisplay : MonoBehaviour 
    { 
    
    }

    public class ObjectiveDisplay : MonoBehaviour 
    {
    
    }
}