using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Ability_", menuName = "Curry/Ability/New Ability Resource", order = 1)]
    public class AbilityResource : ScriptableObject 
    {
        [SerializeField] protected AbilityContent m_content = default;
        public AbilityContent Content => m_content;
    }
}
