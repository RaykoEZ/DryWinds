using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    public struct SandstormProperty : RadialCrisisProperty
    {
        [SerializeField] float m_life;
        [Range(0f, 1f)]
        [SerializeField] float m_intensity;
        [SerializeField] float m_startRadius;
        [SerializeField] float m_growthRate;
        public float Life { get { return m_life; } private set { m_life = value; } }
        public float Intensity { get { return m_intensity; } private set { m_intensity = value; } }
        public float StartRadius { get { return m_startRadius; } private set { m_startRadius = value; } }
        public float GrowthRate { get { return m_growthRate; } private set { m_growthRate = value; } }

        public SandstormProperty(float life, float intensity, 
            float startRadius, float growthRate)
        {
            m_life = life;
            m_intensity = intensity;
            m_startRadius = startRadius;
            m_growthRate = growthRate;
        }
    }
}
