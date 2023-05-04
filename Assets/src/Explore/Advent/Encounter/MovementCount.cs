using UnityEngine;
using System;
using TMPro;

namespace Curry.Explore
{
    [Serializable]
    public class MovementCount 
    {
        [SerializeField] int m_maxMoveCount = default;

        int m_currentMoveCount = 0;
        public int Current => m_currentMoveCount;
        public int Max => m_maxMoveCount;

        public void Init() 
        {
            m_currentMoveCount = m_maxMoveCount;
        }
        public void UpdateCount(int add = 1) 
        {
            m_currentMoveCount += add;
            m_currentMoveCount = Mathf.Clamp(m_currentMoveCount, 0, m_maxMoveCount);
        }
    }

}
