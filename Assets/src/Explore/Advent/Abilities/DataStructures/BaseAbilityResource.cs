using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseAbilityResource : ScriptableObject 
    {
        [SerializeField] protected AbilityContent m_content = default;
        public AbilityContent Content => m_content;
    }
}
