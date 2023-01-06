using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public struct AdventurerStats
    {
        [SerializeField] string m_name;
        [SerializeField] int m_hp;
        [Range(1, 3)]
        [SerializeField] int m_moveRange;
        [SerializeField] Transform m_adventurer;
        public string Name { get { return m_name; } }
        public int HP { get { return m_hp; } set { m_hp = value; } }
        public int MoveRange { get { return m_moveRange; } }
        public Vector3 WorldPosition { get { return m_adventurer.position; } }
        public AdventurerStats(AdventurerStats stats) 
        {
            m_name = stats.Name;
            m_hp = stats.HP;
            m_moveRange = stats.MoveRange;
            m_adventurer = stats.m_adventurer;
        }
    }
}
