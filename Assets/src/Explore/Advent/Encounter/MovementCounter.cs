using UnityEngine;
using System;
using TMPro;

namespace Curry.Explore
{
    [Serializable]
    public class MovementCounter 
    {
        [SerializeField] int m_maxMoveCount = default;
        [SerializeField] protected TextMeshProUGUI m_moveCountText = default;

        int m_currentMoveCount = 0;
        public int Current => m_currentMoveCount;
        public void SpendCount(int spend = 1) 
        {
            m_currentMoveCount -= spend;
            m_moveCountText.text = $" {Current} / {m_maxMoveCount}";
        }
        public void AddCount(int add = 1) 
        {
            m_currentMoveCount += add;
            m_moveCountText.text = $" {Current} / {m_maxMoveCount}";
        }
    }

}
