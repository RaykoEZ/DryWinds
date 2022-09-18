﻿using System;
using UnityEngine;
using Curry.UI;

namespace Curry.Explore
{
    public class TimeGauge : MonoBehaviour
    {
        [SerializeField] ResourceBar m_gauge = default;

        public void UpdateTimeLeft(int timeLeft) 
        {
            m_gauge.SetBarValue(timeLeft);
        }
        public void UpdateMaxTime(int maxTime)
        {
            m_gauge.SetMaxValue(maxTime);
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