using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Mod_", menuName = "Curry/Modifiers/New Modifier Content", order = 1)]
    public class ModifierResource : ScriptableObject 
    {
        [SerializeField] ModifierContent m_content = default;
        public virtual ModifierContent Content => m_content;
    }
}
