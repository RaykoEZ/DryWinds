using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class GainTime : PropertyAttribute
    {
        [SerializeField] int m_toGain = default;
        public void ApplyEffect(TimeManager time) 
        {
            time?.AddTime(m_toGain);
        }
        public void ApplyEffect(TimeManager time, int toGain)
        {
            time?.AddTime(toGain);
        }
    }
}