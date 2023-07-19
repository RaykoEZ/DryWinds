using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class StatUp : TacticalModifier
    {
        [SerializeField] TacticalStats m_toGain = default;
        public StatUp(StatUp toCopy) : base(toCopy)
        {
            m_toGain = toCopy.m_toGain;
        }
        protected override TacticalStats Process_Internal(TacticalStats baseVal)
        {
            return baseVal + m_toGain;
        }
    }
}