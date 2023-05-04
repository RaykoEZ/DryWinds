using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class MovementPointChange : PropertyAttribute
    {
        public void ApplyEffect(ICharacter target, int change = 1)
        {
            if(target is IMovementLimiter lim) 
            {
                lim.UpdateMoveLimit(change);
            }
        }
    }
}
