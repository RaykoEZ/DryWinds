using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class HalfHp
    {
        public void ApplyEffect(ICharacter target)
        {
            target.CurrentHp = Mathf.CeilToInt(target.CurrentHp / 2);
        }
    }
}