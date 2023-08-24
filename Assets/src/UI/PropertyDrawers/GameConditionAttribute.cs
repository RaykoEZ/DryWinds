using System;
using UnityEngine;

namespace Curry.Util
{
    [Serializable]
    public class GameConditionAttribute
    {
        [SerializeField] int m_flag = default;
        [SerializeField] GameConditionDatabase m_conditionRef = default;
        public int Flag { get { return m_flag; } set { m_flag = value; } }
        public GameConditionDatabase ConditionSet => m_conditionRef;
    }
}