using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseModifierResource : ScriptableObject 
    {
        [SerializeField] ModifierContent m_content = default;
        public virtual ModifierContent Content => m_content;
    }
}
