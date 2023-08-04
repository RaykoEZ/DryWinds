using Curry.Events;
using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class LoseTime : PropertyAttribute
    {
        [SerializeField] int m_timeLoss = default;
        public void ApplyEffect(TimeManager time)
        {
            time?.TrySpendTime(m_timeLoss);
        }
    }
}