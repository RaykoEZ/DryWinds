using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseAbility : MonoBehaviour, IAbility
    {
        [SerializeField] protected RangeMap m_range = default;
        [SerializeField] protected Sprite m_Icon = default;
        public virtual AbilityContent GetContent() 
        {
            return new AbilityContent
            {
                RangePattern = m_range,
                Icon = m_Icon
            };
        }
    }
}
