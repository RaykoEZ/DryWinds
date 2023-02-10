using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class SwapPosition : ICharacterEffectModule
    {    
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            Vector3 old = target.WorldPosition;
            if (target.Warp(user.WorldPosition))
            {
                user.Warp(old);
            }
        }
    }
}
