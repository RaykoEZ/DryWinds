using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class SetHp : PropertyAttribute
    {
        [SerializeField] int m_setHpTo = default;
        public virtual void ApplyEffect(ICharacter target)
        {
            target.CurrentHp = m_setHpTo;
        }
        public virtual void ApplyEffect(ICharacter target, int setTo)
        {
            target.CurrentHp = setTo;
        }
    }
}