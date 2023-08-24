using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class TemporaryShield : DamageReduction, IDamageModifier, ITimedElement<int>
    {
        [SerializeField] int m_duration = default;
        int m_timeElapsed = 0;
        public int Duration => m_duration;
        public override ModifierContent Content => new ModifierContent
        {
            Icon = base.Content.Icon,
            Name = base.Content.Name,
            Description = $"{base.Content.Description} ({m_duration - m_timeElapsed}s left)"
        };
        public TemporaryShield(int reduction, int duration) : base(reduction)
        {
            m_duration = duration;
            m_timeElapsed = 0;
        }
        public TemporaryShield(TemporaryShield copy) : base(copy)
        {
            m_duration = copy.m_duration;
            m_timeElapsed = copy.m_timeElapsed;
        }
        public void OnTimeElapsed(int dt)
        {
            m_timeElapsed += dt;
            if(m_timeElapsed > m_duration) 
            {
                m_timeElapsed = 0;
                Expire();
            }
        }
    }
}