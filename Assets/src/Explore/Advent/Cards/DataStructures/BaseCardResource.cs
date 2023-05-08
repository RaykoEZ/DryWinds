using System;
using UnityEngine;
using System.Collections;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseCardResource : ScriptableObject 
    {
        [SerializeField] CardAttribute m_attribute = default;
        public CardAttribute Attribute => m_attribute;
    }
}
