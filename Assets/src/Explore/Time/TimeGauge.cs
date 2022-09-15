using System.Collections;
using UnityEngine;
using Curry.UI;

namespace Curry.Explore
{
    public class TimeGauge : MonoBehaviour
    {
        [SerializeField] TimeManager m_timeManager = default;
        [SerializeField] ResourceBar m_gauge = default;
        // Use this for initialization
        void OnEnable()
        {
            m_timeManager.OnTimeSpent += UpdateTime;
        }
        void OnDisable()
        {
            m_timeManager.OnTimeSpent -= UpdateTime;
        }

        void Awake()
        {
            m_gauge.SetMaxValue(m_timeManager.HoursToClear);
            UpdateTime(m_timeManager.HoursToClear);
        }

        void UpdateTime(float hoursLeft) 
        {
            if (hoursLeft <= m_timeManager.HoursToClear) 
            {
                m_gauge.SetBarValue(hoursLeft);
            }
        }
    }
}