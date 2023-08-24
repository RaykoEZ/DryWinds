using System;
using UnityEngine;

namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "AreaAttack_", menuName = "Curry/Effects/AreaAttack", order = 1)]
    public class AreaAttack_EffectResource : BaseEffectResource
    {       
        [SerializeField] AreaAttack m_effect = default;
        public AreaAttack Effect => m_effect;

    }
}