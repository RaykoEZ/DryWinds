using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class HealingModule : PropertyAttribute
    {
        [SerializeField] int m_healAmount = default;
        public int HealAmount => m_healAmount;
        public void ApplyEffect(ICharacter target)
        {
            target.Recover(m_healAmount);
        }
        public void ApplyEffect(ICharacter target, int healAmount) 
        {
            target.Recover(healAmount);
        }
    }
}
