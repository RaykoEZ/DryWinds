using Curry.Events;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class SwapPosition : PropertyAttribute
    {    
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            Vector3 old = user.WorldPosition;
            if (user.Warp(target.WorldPosition))
            {
                target.Warp(old);
            }
        }
    }
}
