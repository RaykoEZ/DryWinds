using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseAbility : MonoBehaviour, IAbility
    {
        [SerializeField] protected Sprite m_Icon = default;
        protected abstract RangeMap Range { get; }
        public virtual AbilityContent GetContent() 
        {
            return new AbilityContent
            {
                RangePattern = Range,
                Icon = m_Icon
            };
        }
    }
}
