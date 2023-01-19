using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public struct AdventurerStats
    {
        [Range(1, 3)]
        [SerializeField] int m_moveRange;
        [SerializeField] Transform m_adventurer;
        public int MoveRange { get { return m_moveRange; } }
        public Vector3 WorldPosition { get { return m_adventurer.position; } }
        public AdventurerStats(AdventurerStats stats) 
        {
            m_moveRange = stats.MoveRange;
            m_adventurer = stats.m_adventurer;
        }
    }
}
