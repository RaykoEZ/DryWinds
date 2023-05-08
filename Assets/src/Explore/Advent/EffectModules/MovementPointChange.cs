using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class MovementPointChange : PropertyAttribute
    {
        [SerializeField] int m_changeValue = default;
        public void ApplyEffect(ICharacter target)
        {
            if(target is IMovementLimiter lim) 
            {
                lim.UpdateMoveLimit(m_changeValue);
            }
        }
    }
}
