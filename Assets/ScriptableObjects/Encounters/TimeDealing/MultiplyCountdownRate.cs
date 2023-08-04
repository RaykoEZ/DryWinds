using Curry.UI;
using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class MultiplyCountdownRate : PropertyAttribute
    {
        [SerializeField] float m_multiplier = default;
        public void ApplyEffect(TimeManager time)
        {
            time.MultiplyCountdownSpeed(m_multiplier);
        }
        public void ApplyEffect(TimeManager time, float mult)
        {
            time.MultiplyCountdownSpeed(mult);
        }
    }
}