using System;
using UnityEngine;

namespace Curry.Util
{
    [Serializable]
    public class GameConditionAttribute
    {
        [SerializeField] int m_flag = default;
        [SerializeField] GameConditionDatabase m_conditionRef = default;
        public int Flag => m_flag;
        public GameConditionDatabase ConditionSet => m_conditionRef;
    }
}