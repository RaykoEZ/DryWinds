using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class HealingModule : PropertyAttribute, ICharacterEffectModule
    {
        [SerializeField] int m_healAmount = default;
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            target.Recover(m_healAmount);
        }
    }
}
